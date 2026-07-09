import { useMutation } from '@tanstack/react-query';
import type { LoginForm } from '../schemas/authSchema';
import api from '@/api/client';
import { ENDPOINTS } from '@/api/endpoints';

export const useLogin = () => {
  return useMutation({
    mutationFn: async (data: LoginForm) => {
      const res = await api.post(
        ENDPOINTS.identity.login,
        data
      );

      return res.data;
    },

    onSuccess: data => {
      localStorage.setItem(
        'accessToken',
        data.accessToken
      );

      localStorage.setItem(
        'refreshToken',
        data.refreshToken
      );
    },
  });
};