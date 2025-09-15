import { Metadata } from "next";
import ForbiddenPage from "./components/ForbiddenPage";

export const metadata: Metadata = {
  title: "Доступ запрещен | Freaks Guild",
  description: "У вас нет прав доступа к этой странице",
};

export default function Page() {
  return <ForbiddenPage />;
}
