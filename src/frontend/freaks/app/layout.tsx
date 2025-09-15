import type { Metadata } from "next";
import "@/style/global.scss";
import { AppProviders } from "./providers";

export const metadata: Metadata = {
  title: "",
  description: "",
};

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <head></head>
      <body>
        <AppProviders>{children}</AppProviders>
      </body>
    </html>
  );
}
