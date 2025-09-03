import { create } from "zustand";

interface AuthTokenState {
  accessToken: string | null;
  idToken: string | null;
  setTokens: (tokens: {
    accessToken: string | null;
    idToken: string | null;
  }) => void;
}

export const useTokens = create<AuthTokenState>((set) => ({
  accessToken: null,
  idToken: null,
  setTokens: ({ accessToken, idToken }) => set({ accessToken, idToken }),
}));
