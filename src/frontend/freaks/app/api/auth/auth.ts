/* eslint-disable @typescript-eslint/no-explicit-any */
import NextAuth from "next-auth";
import KeycloakProvider from "next-auth/providers/keycloak";
import type { JWT } from "next-auth/jwt";
import axios from "axios";
import { handleAxiosError } from "@/shared/errors";

const keycloakTokenUrl = `${process.env.NEXT_PUBLIC_KEYCLOAK_ISSUER}/protocol/openid-connect/token`;

/** Собираем роли из profile (realm + client) */
function extractRolesFromProfile(p: any) {
  const set = new Set<string>();
  const clientId = process.env.KEYCLOAK_ID!;
  p?.realm_access?.roles?.forEach((r: string) => set.add(r));
  p?.resource_access?.[clientId]?.roles?.forEach((r: string) => set.add(r));
  // если ты добавишь плоский массив roles в userinfo — тоже подхватим:
  if (Array.isArray(p?.roles)) p.roles.forEach((r: string) => set.add(r));
  return [...set];
}

async function refreshAccessToken(token: JWT): Promise<JWT> {
  try {
    const params = new URLSearchParams({
      client_id: process.env.KEYCLOAK_ID!,
      grant_type: "refresh_token",
      refresh_token: token.refreshToken as string,
    });

    const resp = await axios.post(keycloakTokenUrl, params.toString(), {
      headers: { "Content-Type": "application/x-www-form-urlencoded" },
    });
    const data = resp.data;

    return {
      ...token,
      accessToken: data.access_token,
      accessTokenExpires: Date.now() + data.expires_in * 1000,
      refreshToken: data.refresh_token ?? token.refreshToken,
      idToken: data.id_token ?? token.idToken,
      // game_nickname/roles остаются прежними до следующего логина
      // (если нужно обновлять на рефреше — можно дополнить декодом id/access token здесь)
    };
  } catch (error) {
    console.error("🔄 RefreshAccessTokenError", handleAxiosError(error));
    return { ...token, error: "RefreshAccessTokenError" };
  }
}

export const { handlers, auth } = NextAuth({
  trustHost: true,
  providers: [
    KeycloakProvider({
      clientId: process.env.KEYCLOAK_ID!,
      issuer: process.env.NEXT_PUBLIC_KEYCLOAK_ISSUER!,
      // Просим нужные скоупы (если не повешены как default клиент-скоупы)
      authorization: { params: { scope: "openid profile email roles" } },
      // Забираем клеймы из userinfo/id_token и формируем user-объект
      profile(profile) {
        return {
          id: profile.sub,
          name: profile.preferred_username ?? profile.name,
          email: profile.email,
          game_nickname: profile.game_nickname, // <- из маппера User Attribute
          roles: extractRolesFromProfile(profile), // <- роли (realm + client)
        } as any;
      },
    }),
  ],
  session: { strategy: "jwt" },
  callbacks: {
    async jwt({ token, user, account }) {
      if (account && user) {
        return {
          ...token,
          accessToken: account.access_token,
          refreshToken: account.refresh_token!,
          idToken: account.id_token,
          accessTokenExpires: (account.expires_at ?? 0) * 1000,

          game_nickname: (user as any).game_nickname,
          roles: (user as any).roles ?? [],
          email: (user as any).email ?? token.email,
          preferred_username:
            (user as any).name ??
            (user as any).preferred_username ??
            (token as any).preferred_username,
        };
      }

      if (Date.now() < (token.accessTokenExpires as number)) return token;
      return refreshAccessToken(token);
    },

    async session({ session, token }) {
      (session as any).accessToken = token.accessToken;
      (session as any).idToken = token.idToken;

      session.user = session.user ?? {};
      (session.user as any).email = (token as any).email ?? session.user.email;
      (session.user as any).preferred_username = (
        token as any
      ).preferred_username;
      (session.user as any).game_nickname = (token as any).game_nickname;
      (session.user as any).roles = (token as any).roles ?? [];

      return session;
    },
  },
});
