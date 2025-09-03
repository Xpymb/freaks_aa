"use client";

import { useRouter } from "next/navigation";
import { signIn, signOut } from "next-auth/react";
import { mutate } from "swr";

type UseAuthActionsParams = {
  isAuthenticated: boolean;
  idToken?: string | null;
};

export const useAuthActions = ({
  isAuthenticated,
  idToken,
}: UseAuthActionsParams) => {
  const router = useRouter();

  const handleAuth = () => {
    if (isAuthenticated) {
      router.push("/profile");
    } else {
      signIn("keycloak", { redirectTo: "/profile" });
    }
  };

  const handleLogout = async () => {
    try {
      await mutate("/profile/", null, false);
      sessionStorage.removeItem("selectedSectionId");

      const issuer = process.env.NEXT_PUBLIC_KEYCLOAK_ISSUER!;
      const currentPath = window.location.pathname;
      const redirectUrl =
        currentPath === "/profile"
          ? window.location.origin
          : window.location.href;

      const logoutUrl = new URL(`${issuer}/protocol/openid-connect/logout`);
      logoutUrl.searchParams.set("id_token_hint", idToken ?? "");
      logoutUrl.searchParams.set("post_logout_redirect_uri", redirectUrl);

      await signOut({ redirect: false });
      window.location.href = logoutUrl.toString();
    } catch (err) {
      console.error("Logout error", err);
    }
  };

  return {
    handleAuth,
    handleLogout,
  };
};
