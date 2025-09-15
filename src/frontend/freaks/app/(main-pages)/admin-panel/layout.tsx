import { ReactNode } from "react";
import { ServerRoleGuard } from "@/components/guards/ServerRoleGuard";

type Props = {
  children: ReactNode;
};

export default async function AdminPanelLayout({ children }: Props) {
  return <ServerRoleGuard requiredRoles="admin">{children}</ServerRoleGuard>;
}
