import axios from 'axios';
import type { AxiosError, InternalAxiosRequestConfig } from 'axios';

// Định nghĩa cấu trúc lưu trữ các Request bị tạm dừng khi token đang refresh
interface FailedRequestQueueItem {
  resolve: (token: string) => void;
  reject: (error: any) => void;
}

let isRefreshing = false;
let failedQueue: FailedRequestQueueItem[] = [];

// Hàm xử lý việc gửi tiếp các request đang bị đợi sau khi đã có token mới
const processQueue = (error: any, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (token) {
      prom.resolve(token);
    } else {
      prom.reject(error);
    }
  });
  failedQueue = [];
};

export const  api = axios.create({
  baseURL: import.meta.env.VITE_API_GATEWAY_URL,
  timeout: Number(import.meta.env.VITE_API_TIMEOUT ?? 10000),
  withCredentials: true, // BẮT BUỘC: Cho phép tự động đính kèm và nhận HttpOnly Cookie
  headers: {
    'Content-Type': 'application/json',
  },
});


// 1. REQUEST INTERCEPTOR: Tự động đính kèm AccessToken vào Header
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // Lấy Access Token từ bộ nhớ trong (Zustand / Redux / Local State)
    const token = getInMemoryAccessToken(); 
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// 2. RESPONSE INTERCEPTOR: Xử lý xoay vòng token tự động khi gặp lỗi 401
api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

    if (!originalRequest) return Promise.reject(error);

    // Nếu mã lỗi là 401 (Hết hạn Access Token) và request này chưa từng được thử lại trước đó
    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        // Nếu một tiến trình refresh đang chạy, đưa request hiện tại vào hàng đợi
        return new Promise((resolve, reject) => {
          failedQueue.push({
            resolve: (token: string) => {
              if (originalRequest.headers) {
                originalRequest.headers.Authorization = `Bearer ${token}`;
              }
              resolve(api(originalRequest));
            },
            reject: (err: any) => reject(err),
          });
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        // Gọi âm thầm lên API Gateway để Refresh (Trình duyệt sẽ tự động đính kèm HttpOnly Cookie)
        const response = await axios.post(
          'http://localhost:5000/api/auth/refresh',
          {},
          { withCredentials: true } // Vẫn phải chỉ định rõ ở axios thô
        );

        const result = response.data;
        if (result.isSuccess) {
          const newAccessToken = result.value.accessToken;
          
          // Cập nhật lại token mới vào In-Memory State của ứng dụng
          setInMemoryAccessToken(newAccessToken); 

          // Giải phóng toàn bộ hàng đợi bằng token mới
          processQueue(null, newAccessToken);

          // Cập nhật header cho request gốc hiện tại và chạy lại nó
          if (originalRequest.headers) {
            originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
          }
          return api(originalRequest);
        }
      } catch (refreshError) {
        // Nếu refresh thất bại (Refresh Token cũng hết hạn), bắt buộc đăng xuất người dùng
        processQueue(refreshError, null);
        handleForceLogout();
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

// use local storage.

// api.interceptors.request.use(config => {
//   const token = localStorage.getItem('accessToken');

//   if (token) {
//     config.headers.Authorization = `Bearer ${token}`;
//   }

//   return config;
// });


// --- Các hàm phụ trợ quản lý State (In-Memory) ---
let _accessToken: string | null = null;

export const getInMemoryAccessToken = () => _accessToken;
export const setInMemoryAccessToken = (token: string | null) => {
  _accessToken = token;
};

const handleForceLogout = () => {
  setInMemoryAccessToken(null);
  // Điều hướng người dùng về trang đăng nhập và xóa các dữ liệu cục bộ khác
  window.location.href = '/login';
};
