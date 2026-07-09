// store/authStore.ts
import { create } from 'zustand';

interface User { id: string; email: string; roles: string[]; }

interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  login: (user: User) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  isAuthenticated: false,
  login: (user) => set({ user, isAuthenticated: true }),
  logout: () => {
    localStorage.clear();
    set({ user: null, isAuthenticated: false });
  },
}));