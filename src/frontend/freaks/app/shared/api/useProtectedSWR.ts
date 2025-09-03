"use client";

import useSWR, { SWRConfiguration, SWRResponse } from "swr";
import { useAppError } from "../errors/useAppError";
import { useTokens } from "@/store/authTokenStore";
import { mutateSession } from "@/store/SessionStoreProvider";
import { signOut } from "next-auth/react";

type ProtectedSWRResponse<T> = SWRResponse<T, any> & {
  errorState: ReturnType<typeof useAppError>;
};

const defaultConfig: SWRConfiguration = {
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

export function useProtectedSWR<T>(
  key: string | null,
  fetcher: (token: string) => Promise<T>,
  config?: SWRConfiguration
): ProtectedSWRResponse<T> {
  const { accessToken } = useTokens();
  const shouldFetch = !!accessToken && !!key;

  const swr = useSWR<T>(
    shouldFetch ? key : null,
    () => {
      const accessToken = useTokens.getState().accessToken;
      return fetcher(accessToken!);
    },
    {
      ...defaultConfig,
      ...config,
    }
  );

  const errorState = useAppError(swr.error, !!swr.data);

  return {
    ...swr,
    errorState,
  };
}
