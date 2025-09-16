import NextAuth from "next-auth";

declare module "next-auth" {
  interface User {
    id: string;
    game_nickname?: string;
    username?: string;
    roles: string[];
  }
  interface Session {
    user: User;
    accessToken?: string;
    idToken?: string;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    accessToken?: string;
    refreshToken?: string;
    idToken?: string;
    accessTokenExpires?: number;
    id?: string;
    game_nickname?: string;
    roles?: string[];
    preferred_username?: string;
    error?: string;
  }
}
