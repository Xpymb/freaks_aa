// "use client";

// import { useEffect } from "react";
// import { useAuth } from "@/domains/auth";
// import { signIn } from "next-auth/react";
// import { useHasRole } from "@/domains/auth";
// import { UserRole } from "@/domains/user/profile";
// import DefaultLoader from "@/components/customComponents/DefaultLoader/DefaultLoader";

// export const AuthGuardClient = ({
//   children,
//   requiredRoles,
// }: {
//   children: React.ReactNode;
//   requiredRoles?: UserRole[];
// }) => {
//   const { isAuthenticated, isLoading } = useAuth();

//   const hasRole = useHasRole(requiredRoles ?? []);

//   useEffect(() => {
//     if (!isLoading && !isAuthenticated) {
//       const callbackUrl = window.location.href;
//       signIn("keycloak", { callbackUrl });
//     }
//   }, [isLoading, isAuthenticated]);

//   if (isLoading || !isAuthenticated) return <DefaultLoader />;

//   if (requiredRoles && !hasRole) {
//     return <div>⛔ Отсутствует доступ к странице</div>;
//   }

//   return <>{children}</>;
// };
