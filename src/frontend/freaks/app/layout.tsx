import type { Metadata } from "next";
import "@/style/global.scss";
import Header from "./layouts/Header/Header";
import { AppProviders } from "./providers";

export const metadata: Metadata = {
  title: "",
  description: "",
};

export default async function Layout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <head></head>
      <body>
        <div className="layoutWrapper">
          <AppProviders>
            <Header />
            <main>
              {/* <Sidebar /> */}
              {children}
            </main>
            {/* <EducationFooter /> */}
          </AppProviders>
        </div>
      </body>
    </html>
  );
}
