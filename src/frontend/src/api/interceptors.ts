//import api from './client';
import { api } from './client';
import { ENDPOINTS } from './endpoints';

api.interceptors.response.use(
  res => res,
  async error => {
    const original = error.config;

    if (
      error.response?.status === 401 &&
      !original._retry
    ) {
      original._retry = true;

      try {
        const refresh = await api.post(
          ENDPOINTS.identity.refresh,
          {
            refreshToken:
              localStorage.getItem('refreshToken'),
          }
        );

        localStorage.setItem(
          'accessToken',
          refresh.data.accessToken
        );

        original.headers.Authorization =
          `Bearer ${refresh.data.accessToken}`;

        return api(original);
      } catch {
        localStorage.clear();
        window.location.href = '/login';
      }
    }

    return Promise.reject(error);
  }
);