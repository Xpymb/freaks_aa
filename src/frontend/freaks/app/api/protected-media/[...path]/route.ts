import { auth } from "@/api/auth/auth";
import { NextRequest } from "next/server";

export async function GET(
  req: NextRequest,
  { params }: { params: { path: string[] } }
) {
  const session = await auth();

  const token = session?.accessToken;

  if (!token) {
    return new Response("Unauthorized", { status: 401 });
  }

  const path = params.path.join("/");
  const backendUrl = `${process.env.PROTECTED_MEDIA_URL}/${path}`;

  const backendRes = await fetch(backendUrl, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!backendRes.ok) {
    return new Response("Access denied", { status: backendRes.status });
  }

  const contentType =
    backendRes.headers.get("content-type") || "application/octet-stream";
  const contentDisposition = backendRes.headers.get("content-disposition");

  const body = backendRes.body!;
  const headers: HeadersInit = { "content-type": contentType };

  if (contentDisposition) headers["content-disposition"] = contentDisposition;

  return new Response(body, {
    headers,
    status: 200,
  });
}
