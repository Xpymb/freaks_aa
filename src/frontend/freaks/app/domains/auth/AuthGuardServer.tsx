import { redirect } from "next/navigation";
import { headers } from "next/headers";
import { ReactNode } from "react";
import { auth } from "@/api/auth/auth";

export async function AuthGuardServer({ children }: { children: ReactNode }) {
  const session = await auth();

  if (!session) {
    const headerList = await headers();
    const raw = headerList.get("x-forwarded-uri") || "/";
    const callbackUrl = encodeURIComponent(raw);
    redirect(`/login?callbackUrl=${callbackUrl}`);
  }

  return <>{children}</>;
}
