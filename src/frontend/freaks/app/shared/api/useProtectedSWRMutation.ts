"use client";

import useSWRMutation, {
  SWRMutationConfiguration,
  SWRMutationResponse,
} from "swr/mutation";
import { useTokens } from "@/store/authTokenStore";
import { mutateSession } from "@/store/SessionStoreProvider";
import { signOut } from "next-auth/react";
import { useAppError } from "../errors/useAppError";

import { AppError } from "../errors/appErrorConstructor";
import { handleAxiosError } from "../errors";

type ProtectedSWRMutationResponse<T, Args> = SWRMutationResponse<
  T,
  AppError,
  string,
  Args
> & {
  errorState: ReturnType<typeof useAppError>;
};

const getStatus = (e: unknown) => handleAxiosError(e).status;
const is401 = (e: unknown) => getStatus(e) === 401;

// обёртка: повтор после refresh при 401 + нормализация ошибок в AppError
function with401Retry<T, Args>(fn: (token: string, args: Args) => Promise<T>) {
  return async (token: string, args: Args): Promise<T> => {
    try {
      return await fn(token, args);
    } catch (err) {
      if (!is401(err)) throw handleAxiosError(err);

      try {
        await mutateSession();
        const newToken = useTokens.getState().accessToken;
        if (!newToken) throw handleAxiosError(err);
        return await fn(newToken, args);
      } catch (refreshErr) {
        // refresh не удался — разлогиниваем и кидаем нормализованную ошибку
        signOut();
        throw handleAxiosError(refreshErr);
      }
    }
  };
}

/**
 * Мутационный SWR-хук с авто-токеном, retry при 401 и AppError.
 * T — результат, Args — аргументы мутации.
 */
export function useProtectedSWRMutation<T, Args>(
  key: string,
  fetcher: (token: string, args: Args) => Promise<T>,
  config?: SWRMutationConfiguration<T, AppError, string, Args>
): ProtectedSWRMutationResponse<T, Args> {
  const run = with401Retry(fetcher);

  const swr = useSWRMutation<T, AppError, string, Args>(
    key,
    async (_key, { arg }) => {
      const token = useTokens.getState().accessToken;
      if (!token) {
        throw new AppError("No access token", "AUTH_ERROR");
      }
      return run(token, arg);
    },
    config
  );

  const errorState = useAppError(swr.error, false);
  return { ...swr, errorState };
}
