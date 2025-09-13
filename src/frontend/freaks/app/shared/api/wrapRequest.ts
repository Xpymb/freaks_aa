import { AxiosResponse } from "axios";
import { handleAxiosError } from "@/shared/errors";

export async function wrapRequest<T>(
  fn: () => Promise<AxiosResponse<T>>
): Promise<T> {
  try {
    const { data } = await fn();
    return data;
  } catch (error: unknown) {
    throw handleAxiosError(error);
  }
}
