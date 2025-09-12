"use client";

import useSWR, { Key, SWRConfiguration, SWRResponse } from "swr";
import { useAppError } from "../errors/useAppError";
import { useTokens } from "@/store/authTokenStore";
import { mutateSession } from "@/store/SessionStoreProvider";
import { signOut } from "next-auth/react";

export type HttpError = {
  status?: number;
  message?: string;
  code?: string | number;
};

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

/**
 * Типобезопасный врапер над SWR с автоматической авторизацией.
 * T — тип данных, E — тип ошибки (по умолчанию HttpError).
 */
export function useProtectedSWR<T, E = HttpError>(
  key: Key,
  fetcher: (token: string) => Promise<T>,
  config?: SWRConfiguration<T, E>
): ProtectedSWRResponse<T, E> {
  const { accessToken } = useTokens();
  const shouldFetch = !!accessToken && !!key;

  const mergedConfig: SWRConfiguration<T, E> = {
    ...(defaultConfig as SWRConfiguration<T, E>),
    ...(config ?? {}),
  };

  const swr = useSWR<T, E>(
    shouldFetch ? key : null,
    async () => {
      const token = useTokens.getState().accessToken;
      if (!token) throw new Error("No access token");
      return fetcher(token);
    },
    mergedConfig
  );

  const errorState = useAppError(swr.error, !!swr.data);

  return {
    ...swr,
    errorState,
  };
}
