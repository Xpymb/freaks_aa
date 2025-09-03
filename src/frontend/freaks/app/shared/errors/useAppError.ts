import { AppError } from "@/shared/errors/appErrorConstructor";

export function useAppError(error: unknown, hasData = false) {
  const isAppError = error instanceof AppError;

  if (!isAppError) {
    return {
      isError: !hasData && !!error,
      status: null,
      code: null,
      details: null,
      message: (error as Error)?.message ?? null,
      isUnlinked: false,
      isValidationError: false,
      isServerError: false,
    };
  }

  const status = error.status;

  return {
    isError: !hasData,
    status,
    code: error.code ?? null,
    details: error.details ?? null,
    message: error.message,
    isUnlinked: status === 404,
    isValidationError: status === 400,
    isServerError: status === 500 || status === 503,
  };
}
