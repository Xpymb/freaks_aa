import { Metadata } from "next";
import OverviewPage from "./components/OverviewPage";

export const metadata: Metadata = {
  title: "Обзор | Freaks Guild",
  description: "Главная страница гильдии Freaks с обзором рейдов и статистикой",
};

export default function Page() {
  return <OverviewPage />;
}
