"use client";

import { useEffect } from "react";
import { signOut } from "next-auth/react";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";

export default function LogoutPage() {
  useEffect(() => {
    const callbackUrl =
      new URLSearchParams(window.location.search).get("callbackUrl") || "/";
    signOut({ callbackUrl });
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
