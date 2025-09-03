import { headers } from "next/headers";
import { handleAxiosError } from "@/shared/errors";
import { redirect } from "next/navigation";

export async function requestServer<T>(
  fn: () => Promise<T>
): Promise<T | null> {
  try {
    return await fn();
  } catch (error: any) {
    const status = error?.response?.status ?? error?.status;

    if (status === 401) {
      const headerList = headers();
      const callbackUrl = headerList.get("x-forwarded-uri") || "/";
      redirect(`/logout?callbackUrl=${callbackUrl}`);

      return null;
    }

    throw handleAxiosError(error);
  }
}
