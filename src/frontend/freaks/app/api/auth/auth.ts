import NextAuth from "next-auth";
import KeycloakProvider from "next-auth/providers/keycloak";
import type { JWT } from "next-auth/jwt";
import axios from "axios";
import { handleAxiosError } from "@/shared/errors";
import { IKeycloakUser } from "@/types";

const keycloakTokenUrl = `${process.env.NEXT_PUBLIC_KEYCLOAK_ISSUER}/protocol/openid-connect/token`;
const keycloakUserInfoUrl = `${process.env.NEXT_PUBLIC_KEYCLOAK_ISSUER}/protocol/openid-connect/userinfo`;

async function getUserInfo(accessToken: string) {
  try {
    const response = await axios.get(keycloakUserInfoUrl, {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
    return response.data;
  } catch {
    return null;
  }
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
      // Сохраняем ID, game_nickname и roles при рефреше токена
      id: token.id,
      game_nickname: token.game_nickname,
      roles: token.roles,
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
      authorization: { params: { scope: "openid profile email roles" } },
      profile(profile: IKeycloakUser) {
        return {
          id: profile.sub,
          game_nickname: profile.game_nickname,
          username: profile.preferred_username,
          roles: profile.roles || [],
        };
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
          id: user.id,
          game_nickname: user.game_nickname,
          username: user.username,
          roles: user.roles ?? [],
        };
      }

      if (Date.now() < (token.accessTokenExpires as number)) return token;
      return refreshAccessToken(token);
    },

    async session({ session, token }) {
      let currentToken = token;
      session.user = session.user ?? {};
      if (token.accessToken) {
        if (Date.now() >= (token.accessTokenExpires as number)) {
          currentToken = await refreshAccessToken(token);
        }

        const userInfo = await getUserInfo(currentToken.accessToken as string);

        if (userInfo) {
          session.user.id = userInfo.sub;
          session.user.username = userInfo.preferred_username;
          session.user.game_nickname = userInfo.game_nickname;
          session.user.roles = userInfo.roles || [];
        } else {
          let actualUserId = token.id as string;
          try {
            const parts = (token.accessToken as string).split(".");
            if (parts.length === 3) {
              const payload = JSON.parse(atob(parts[1]));
              actualUserId = payload.sub;
            }
          } catch {}
          session.user.id = actualUserId;
          session.user.username = token.preferred_username;
          session.user.game_nickname = token.game_nickname;
          session.user.roles = token.roles ?? [];
        }
      }
      session.accessToken = currentToken.accessToken;
      session.idToken = currentToken.idToken;
      return session;
    },
  },
});
