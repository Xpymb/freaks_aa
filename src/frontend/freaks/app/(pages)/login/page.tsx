"use client";

import { useEffect } from "react";
import { signIn } from "next-auth/react";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";
export default function LoginPage() {
  useEffect(() => {
    const callbackUrl =
      new URLSearchParams(window.location.search).get("callbackUrl") || "/";
    signIn("keycloak", { callbackUrl });
  }, []);

  return (
    <section
      style={{
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        height: "100vh",
      }}
    >
      <DefaultLoader />
    </section>
  );
}
