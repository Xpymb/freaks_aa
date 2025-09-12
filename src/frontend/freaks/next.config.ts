import type { NextConfig } from "next";

// Загружаем переменные окружения
const STORAGE_URL = process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL;

if (!STORAGE_URL) {
  throw new Error("NEXT_PUBLIC_STORAGE_MEDIA_URL not defined");
}

const storageUrl = new URL(STORAGE_URL);

const nextConfig: NextConfig = {
  images: {
    remotePatterns: [
      {
        protocol: storageUrl.protocol.replace(":", "") as "http" | "https",
        hostname: storageUrl.hostname,
        ...(storageUrl.port ? { port: storageUrl.port } : {}),
        pathname: "/**",
      },
    ],
  },
};

export default nextConfig;
