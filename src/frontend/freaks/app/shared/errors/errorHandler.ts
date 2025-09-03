import axios from "axios";
import { AppError } from "./appErrorConstructor";

export const handleAxiosError = (error: unknown): AppError => {
  if (!axios.isAxiosError(error)) {
    return new AppError("Неизвестная ошибка", "UNKNOWN_ERROR");
  }

  const { response, request } = error;

  if (response) {
    const { status, data } = response;
    return new AppError(
      data?.message || "Ошибка сервера",
      data?.code || "SERVER_ERROR",
      data?.details,
      status,
      error
    );
  }

  if (request) {
    return new AppError(
      "Сервер не отвечает",
      "NETWORK_ERROR",
      undefined,
      undefined,
      error
    );
  }

  return new AppError(
    "Неизвестная ошибка",
    "UNKNOWN_ERROR",
    undefined,
    undefined,
    error
  );
};
