import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './features/auth/pages/Login';
import { useEffect, useState } from 'react';
import {api, setInMemoryAccessToken} from '@/api/client';
import { ENDPOINTS } from './api/endpoints';
// Giả sử anh có một màn hình Dashboard hoặc Home sau khi login, 
// nếu chưa có anh có thể thay thế sau nhé.
function App() {
  const [isInitializing, setIsInitializing] = useState(true);
  useEffect(() => {
    console.log("refresh called");
    const initializeAuth = async () => {
      try {
        // Gọi API /api/auth/refresh ngay khi app vừa mount để kiểm tra HttpOnly Cookie
        const response = await api.post(ENDPOINTS.identity.refresh);
        
        // Kiểm tra cấu trúc Result Pattern giống backend đang trả về
        if (response.data && response.data.isSuccess) {
          setInMemoryAccessToken(response.data.value.accessToken);
        }
      } catch (error) {
        console.log('Chưa đăng nhập hoặc phiên làm việc đã hết hạn (Silent Refresh khởi tạo thất bại).');
        console.log(error);
      } finally {
        setIsInitializing(false); // Hoàn tất quá trình khôi phục session, cho phép render UI chính
      }
    };

    initializeAuth();
  }, []);

  // Trong lúc đang gọi API check cookie ngầm, hiển thị màn hình chờ để tránh hiện tượng nháy giao diện (UX chuẩn)
  if (isInitializing) {
      return (
      <div className="flex h-screen w-screen items-center justify-center bg-gray-50">
        <div className="text-center font-medium text-gray-600">
          Đang tải ứng dụng...
        </div>
      </div>
    );
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/" element={<Navigate to="/login" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;