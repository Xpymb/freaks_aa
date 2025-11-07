import { NextResponse } from "next/server";
import { auth } from "@/api/auth/auth";
import type { UserRole } from "@/types/roles.types";
import { AVAILABLE_ROLES } from "@/types/roles.types";

// Публичные роуты - без авторизации
const PUBLIC_PREFIXES = [
  "/login",
  "/logout",
  "/api/auth",
  "/_next",
  "/favicon.ico",
  "/robots.txt",
  "/sitemap.xml",
];

// Роуты только с проверкой авторизации (без проверки ролей)
const AUTH_ONLY_PREFIXES = [
  "/forbidden",
];

// Маршруты, требующие специальных ролей
const ROLE_PROTECTED_ROUTES: Record<string, UserRole[]> = {
  "/admin-panel": ["admin", "guild_leader"],
  // Добавить другие защищенные маршруты по мере необходимости
  // "/guild-management": ["admin", "guild_leader"],
  // "/editor-panel": ["admin", "guild_leader", "editor"],
};

function isPublic(pathname: string) {
  return PUBLIC_PREFIXES.some((p) => pathname.startsWith(p));
}

function isAuthOnly(pathname: string) {
  return AUTH_ONLY_PREFIXES.some((p) => pathname.startsWith(p));
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

function hasValidRole(userRoles: UserRole[]): boolean {
  return userRoles.some((role) => AVAILABLE_ROLES.includes(role));
}

export default auth((request) => {
  const { pathname, search } = request.nextUrl;
  const headers = new Headers(request.headers);

  if (process.env.NODE_ENV == "development") {
    headers.set("x-forwarded-uri", request.nextUrl.pathname);
  }

  // 1. PUBLIC роуты → пропустить без проверок
  if (isPublic(pathname)) {
    return NextResponse.next({ headers });
  }

  // 2. Нет авторизации → редирект на /login
  if (!request.auth) {
    const url = new URL("/login", request.nextUrl);
    url.searchParams.set("callbackUrl", pathname + (search || ""));
    return NextResponse.redirect(url);
  }

  // 3. AUTH_ONLY роуты → пропустить (авторизация уже проверена)
  if (isAuthOnly(pathname)) {
    return NextResponse.next({ headers });
  }

  // 4. PROTECTED роуты → проверка ролей
  const userRoles = (request.auth.user?.roles || []) as UserRole[];

  // Проверяем что у юзера есть хоть одна валидная роль
  if (!hasValidRole(userRoles)) {
    return NextResponse.redirect(new URL("/forbidden", request.nextUrl));
  }

  // 5. Проверяем специфичные требования к ролям для конкретных роутов
  const requiredRoles = getRequiredRoles(pathname);
  if (requiredRoles) {
    if (!hasRequiredRole(userRoles, requiredRoles)) {
      return NextResponse.redirect(new URL("/forbidden", request.nextUrl));
    }
  }

  return NextResponse.next({ headers });
});

export const config = {
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};
