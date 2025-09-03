import ThemeRegistry from "./ThemeMUI/ThemeRegistry";
import SessionStoreProvider from "./store/SessionStoreProvider";

type Props = {
  children: React.ReactNode;
};

export function AppProviders({ children }: Props) {
  return (
    <>
      <SessionStoreProvider />
      <ThemeRegistry>{children}</ThemeRegistry>
    </>
  );
}
