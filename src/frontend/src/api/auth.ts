import api from './client';
import { ENDPOINTS } from './endpoints';

export const refreshToken = async () => {
  const refreshTokenValue = localStorage.getItem('refreshToken');

  const response = await api.post(
    ENDPOINTS.identity.refresh,
    {
      refreshToken: refreshTokenValue,
    }
  );

  return response.data;
};