import { NextResponse } from "next/server";
import { auth } from "@/api/auth/auth";

const PUBLIC_PREFIXES = [
  "/login",
  "/logout",
  "/api/auth",
  "/_next",
  "/favicon.ico",
  "/robots.txt",
  "/sitemap.xml",
];

function isPublic(pathname: string) {
  return PUBLIC_PREFIXES.some((p) => pathname.startsWith(p));
}

export default auth((request) => {
  const { pathname, search } = request.nextUrl;

  if (!isPublic(pathname) && !request.auth) {
    const url = new URL("/login", request.nextUrl);
    url.searchParams.set("callbackUrl", pathname + (search || ""));
    return NextResponse.redirect(url);
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
