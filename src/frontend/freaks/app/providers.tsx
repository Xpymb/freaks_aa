import { SessionProvider } from "next-auth/react";
import ThemeRegistry from "./ThemeMUI/ThemeRegistry";
import SessionStoreProvider from "./store/SessionStoreProvider";
import { SidebarProvider } from "./contexts/SidebarContext";

type Props = {
  children: React.ReactNode;
};

export function AppProviders({ children }: Props) {
  return (
    <SessionProvider>
      <SessionStoreProvider />
      <SidebarProvider>
        <ThemeRegistry>{children}</ThemeRegistry>
      </SidebarProvider>
    </SessionProvider>
  );
}
