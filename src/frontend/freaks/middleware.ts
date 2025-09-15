import { NextResponse } from "next/server";
import { auth } from "@/api/auth/auth";
import type { UserRole } from "@/types/roles.types";

const PUBLIC_PREFIXES = [
  "/login",
  "/logout",
  "/api/auth",
  "/_next",
  "/favicon.ico",
  "/robots.txt",
  "/sitemap.xml",
  "/forbidden",
];

// Маршруты, требующие специальных ролей
const ROLE_PROTECTED_ROUTES: Record<string, UserRole[]> = {
  "/admin-panel": ["admin"],
  // Добавить другие защищенные маршруты по мере необходимости
  // "/guild-management": ["admin", "guild_leader"],
  // "/editor-panel": ["admin", "guild_leader", "editor"],
};

function isPublic(pathname: string) {
  return PUBLIC_PREFIXES.some((p) => pathname.startsWith(p));
}

function getRequiredRoles(pathname: string): UserRole[] | null {
  for (const [route, roles] of Object.entries(ROLE_PROTECTED_ROUTES)) {
    if (pathname.startsWith(route)) {
      return roles;
    }
  }
  return null;
}

function hasRequiredRole(
  userRoles: UserRole[],
  requiredRoles: UserRole[]
): boolean {
  return requiredRoles.some((role) => userRoles.includes(role));
}

export default auth((request) => {
  const { pathname, search } = request.nextUrl;

  // Проверка авторизации
  if (!isPublic(pathname) && !request.auth) {
    const url = new URL("/login", request.nextUrl);
    url.searchParams.set("callbackUrl", pathname + (search || ""));
    return NextResponse.redirect(url);
  }

  if (request.auth) {
    const requiredRoles = getRequiredRoles(pathname);
    if (requiredRoles) {
      const userRoles = (request.auth.user?.roles || []) as UserRole[];
      if (!hasRequiredRole(userRoles, requiredRoles)) {
        return NextResponse.redirect(new URL("/forbidden", request.nextUrl));
      }
    }
  }

  const headers = new Headers(request.headers);

  if (process.env.NODE_ENV == "development") {
    headers.set("x-forwarded-uri", request.nextUrl.pathname);
  }

  return NextResponse.next({ headers });
});

export const config = {
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};
