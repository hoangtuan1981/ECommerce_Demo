// hooks/useLogin.ts
import { useMutation } from '@tanstack/react-query';
import api from '@/api/client';
import { LoginForm } from '../schemas';

export const useLogin = () => {
  return useMutation({
    mutationFn: async (data: LoginForm) => {
      const res = await api.post('/auth/login', data);
      return res.data; // { accessToken, refreshToken, user }
    },
    onSuccess: (data) => {
      localStorage.setItem('accessToken', data.accessToken);
      localStorage.setItem('refreshToken', data.refreshToken);
      // update zustand store
    },
  });
};