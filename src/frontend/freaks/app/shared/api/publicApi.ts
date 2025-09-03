import type { AxiosRequestConfig } from "axios";
import { wrapRequest } from "./wrapRequest";
import { baseApi } from "./baseApi";

export const publicApi = {
  get: <T = unknown>(url: string, config?: AxiosRequestConfig): Promise<T> =>
    wrapRequest(() => baseApi.get<T>(url, config)),

  post: <T = unknown, D = unknown>(
    url: string,
    data?: D,
    config?: AxiosRequestConfig
  ): Promise<T> => wrapRequest(() => baseApi.post<T>(url, data, config)),

  put: <T = unknown, D = unknown>(
    url: string,
    data?: D,
    config?: AxiosRequestConfig
  ): Promise<T> => wrapRequest(() => baseApi.put<T>(url, data, config)),

  patch: <T = unknown, D = unknown>(
    url: string,
    data?: D,
    config?: AxiosRequestConfig
  ): Promise<T> => wrapRequest(() => baseApi.patch<T>(url, data, config)),

  delete: <T = unknown>(url: string, config?: AxiosRequestConfig): Promise<T> =>
    wrapRequest(() => baseApi.delete<T>(url, config)),
};
