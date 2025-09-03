import NextAuth from "next-auth";

declare module "next-auth" {
  interface User {
    game_nickname: string;
    roles: string[];
  }
  interface Session {
    user: User;
    accessToken?: string;
    idToken?: string;
  }
}
