import { SessionProvider } from "next-auth/react";
import ThemeRegistry from "./ThemeMUI/ThemeRegistry";
import SessionStoreProvider from "./store/SessionStoreProvider";

type Props = {
  children: React.ReactNode;
};

export function AppProviders({ children }: Props) {
  return (
    <SessionProvider>
      <SessionStoreProvider />
      <ThemeRegistry>{children}</ThemeRegistry>
    </SessionProvider>
  );
}
