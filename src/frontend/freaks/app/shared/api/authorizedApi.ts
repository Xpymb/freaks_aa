import type { AxiosRequestConfig } from "axios";
import { wrapRequest } from "./wrapRequest";
import { baseApi } from "./baseApi";

export const authorizedApi = (token: string) => {
  const authHeaders = {
    Authorization: `Bearer ${token}`,
  };

  return {
    get: <T = unknown>(url: string, config?: AxiosRequestConfig): Promise<T> =>
      wrapRequest(() =>
        baseApi.get<T>(url, {
          ...config,
          headers: {
            ...config?.headers,
            ...authHeaders,
          },
        })
      ),

    post: <T = unknown, D = unknown>(
      url: string,
      data?: D,
      config?: AxiosRequestConfig
    ): Promise<T> =>
      wrapRequest(() =>
        baseApi.post<T>(url, data, {
          ...config,
          headers: {
            ...config?.headers,
            ...authHeaders,
          },
        })
      ),

    put: <T = unknown, D = unknown>(
      url: string,
      data?: D,
      config?: AxiosRequestConfig
    ): Promise<T> =>
      wrapRequest(() =>
        baseApi.put<T>(url, data, {
          ...config,
          headers: {
            ...config?.headers,
            ...authHeaders,
          },
        })
      ),

    patch: <T = unknown, D = unknown>(
      url: string,
      data?: D,
      config?: AxiosRequestConfig
    ): Promise<T> =>
      wrapRequest(() =>
        baseApi.patch<T>(url, data, {
          ...config,
          headers: {
            ...config?.headers,
            ...authHeaders,
          },
        })
      ),

    delete: <T = unknown>(
      url: string,
      config?: AxiosRequestConfig
    ): Promise<T> =>
      wrapRequest(() =>
        baseApi.delete<T>(url, {
          ...config,
          headers: {
            ...config?.headers,
            ...authHeaders,
          },
        })
      ),
  };
};
