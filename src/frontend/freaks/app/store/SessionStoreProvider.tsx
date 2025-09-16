"use client";

import { useEffect, memo } from "react";
import useSWR, { mutate } from "swr";

import type { Session } from "next-auth";
import { useTokens } from "./authTokenStore";
import { fetcher } from "@/utils/fetcher";

const SessionStoreProvider = () => {
  const { setTokens } = useTokens();

  const { data, isLoading } = useSWR<Session | null>(
    "/api/auth/session",
    fetcher,
    {
      refreshInterval: 145 * 1000,
    }
  );

  useEffect(() => {
    if (isLoading || !data?.accessToken || !data.idToken) return;

    setTokens({
      accessToken: data.accessToken,
      idToken: data.idToken,
    });

    fetch(`${process.env.NEXT_PUBLIC_API_URL}/app-version`, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${data.accessToken}`,
      },
    });
  }, [data?.accessToken, data?.idToken, isLoading, setTokens]);

  return null;
};

export default memo(SessionStoreProvider);

export const mutateSession = () => mutate("/api/auth/session");
