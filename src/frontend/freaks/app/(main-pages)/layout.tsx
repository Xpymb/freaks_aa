import { ReactNode } from "react";
import Header from "@/layouts/Header/Header";
import Sidebar from "@/widgets/Sidebar/Sidebar";

type Props = {
  children: ReactNode;
};

export default function MainLayout({ children }: Props) {
  return (
    <div className="layoutWrapper">
      <Header />
      <Sidebar />
      <main className="mainContent">{children}</main>
    </div>
  );
}
