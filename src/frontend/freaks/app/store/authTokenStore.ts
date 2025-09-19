import { create } from "zustand";

interface User {
  id: string;
  game_nickname?: string;
  username?: string;
  roles: string[];
}

interface AuthState {
  accessToken: string | null;
  idToken: string | null;
  user: User | null;
  isAuthenticated: boolean;
  setAuth: (auth: {
    accessToken: string | null;
    idToken: string | null;
    user?: User | null;
  }) => void;
}

export const useAuth = create<AuthState>((set) => ({
  accessToken: null,
  idToken: null,
  user: null,
  isAuthenticated: false,
  setAuth: ({ accessToken, idToken, user }) =>
    set({
      accessToken,
      idToken,
      user: user || null,
      isAuthenticated: !!(accessToken && idToken),
    }),
}));

// Оставляем старый хук для обратной совместимости
export const useTokens = useAuth;
