import type { AxiosRequestConfig } from "axios";
import { wrapRequest } from "./wrapRequest";
import { baseApi, fileApi } from "./baseApi";

export const authorizedApi = (
  token: string,
  serviceType: "file" | "portal"
) => {
  const authHeaders = {
    Authorization: `Bearer ${token}`,
  };

  const apiClient = serviceType === "portal" ? baseApi : fileApi;

  return {
    get: <T = unknown>(url: string, config?: AxiosRequestConfig): Promise<T> =>
      wrapRequest(() =>
        apiClient.get<T>(url, {
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
        apiClient.post<T>(url, data, {
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
        apiClient.put<T>(url, data, {
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
        apiClient.patch<T>(url, data, {
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
        apiClient.delete<T>(url, {
          ...config,
          headers: {
            ...config?.headers,
            ...authHeaders,
          },
        })
      ),
  };
};
