"use client";

import useSWR, { Key, SWRConfiguration, SWRResponse } from "swr";
import { useEffect } from "react";
import { useAppError } from "../errors/useAppError";
import { useAuth } from "@/store/authTokenStore";
import { mutateSession } from "@/store/SessionStoreProvider";
import { signOut } from "next-auth/react";
import type { SSEMessage } from "@/types/sse.types";
import { getChannelClient } from "./sseChannelClient";

export type HttpError = {
  status?: number;
  message?: string;
  code?: string | number;
};

export interface SSEConfig {
  channel: string;
  enabled?: boolean;
  onMessage?: (data: SSEMessage, key: Key, mutate: () => void) => void;
}

interface ProtectedSWRConfig<T, E = HttpError> extends SWRConfiguration<T, E> {
  websocket?: SSEConfig;
}

type ProtectedSWRResponse<T, E = HttpError> = SWRResponse<T, E> & {
  errorState: ReturnType<typeof useAppError>;
};

const defaultConfig: SWRConfiguration<unknown, HttpError> = {
  dedupingInterval: 1000 * 60 * 5,
  revalidateOnFocus: false,
  keepPreviousData: false,
  onErrorRetry: async (error, key, config, revalidate, { retryCount }) => {
    if (error?.status === 401) {
      if (retryCount === 1) {
        try {
          await mutateSession();
          revalidate({ retryCount });
        } catch (refreshErr) {
          console.error("🔥 Failed to refresh token", refreshErr);
        }
      } else if (retryCount > 1) {
        signOut();
      }
    }
  },
};

export function useProtectedSWR<T, E = HttpError>(
  key: Key,
  fetcher: (token: string) => Promise<T>,
  config?: ProtectedSWRConfig<T, E>
): ProtectedSWRResponse<T, E> {
  const { accessToken } = useAuth();
  const shouldFetch = !!accessToken && !!key;

  const mergedConfig: SWRConfiguration<T, E> = {
    ...(defaultConfig as SWRConfiguration<T, E>),
    ...(config ?? {}),
  };

  const swr = useSWR<T, E>(
    shouldFetch ? key : null,
    async () => {
      const token = useAuth.getState().accessToken;
      if (!token) throw new Error("No access token");
      return fetcher(token);
    },
    mergedConfig
  );

  useEffect(() => {
    if (
      !config?.websocket?.enabled ||
      !config?.websocket?.channel ||
      !accessToken
    ) {
      return;
    }

    const channel = config.websocket.channel;
    const client = getChannelClient(channel, accessToken);

    const unsubscribe = client.subscribe((data: SSEMessage) => {
      if (config?.websocket?.onMessage) {
        config.websocket.onMessage(data, key, () => swr.mutate());
      } else {
        swr.mutate();
      }
    });

    return unsubscribe;
  }, [
    key,
    config?.websocket?.channel,
    config?.websocket?.enabled,
    accessToken,
  ]);

  const errorState = useAppError(swr.error, !!swr.data);

  return {
    ...swr,
    errorState,
  };
}
