"use client";

import { useEffect, memo } from "react";
import useSWR, { mutate } from "swr";

import type { Session } from "next-auth";
import { useAuth } from "./authTokenStore";
import { fetcher } from "@/utils/fetcher";
import { disconnectAllChannels } from "@/shared/api/sseChannelClient";

const SessionStoreProvider = () => {
  const { setAuth } = useAuth();

  const { data, isLoading } = useSWR<Session | null>(
    "/api/auth/session",
    fetcher,
    {
      refreshInterval: 145 * 1000,
    }
  );

  useEffect(() => {
    if (isLoading) return;

    // Если нет токенов (logout), очищаем все SSE соединения
    if (!data?.accessToken || !data.idToken) {
      disconnectAllChannels();
      return;
    }

    setAuth({
      accessToken: data.accessToken,
      idToken: data.idToken,
      user: data.user,
    });

    fetch(`${process.env.NEXT_PUBLIC_API_URL}/app-version`, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${data.accessToken}`,
      },
    });
  }, [data?.accessToken, data?.idToken, isLoading]);

  return null;
};

export default memo(SessionStoreProvider);

export const mutateSession = () => mutate("/api/auth/session");
