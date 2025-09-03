import { UserRole } from "@/domains/user/profile";

type RoleMeta = {
  label: string;
  // color: "primary" | "secondary" | "success" | "error";
  // icon: React.ReactNode;
};

export const ROLES_META: Record<UserRole, RoleMeta> = {
  student: {
    label: "Студент",
    // color: "primary",
    // icon: <SchoolIcon fontSize="small" />,
  },
  employee: {
    label: "Сотрудник",
    // color: "success",
    // icon: <WorkIcon fontSize="small" />,
  },
  entrant: {
    label: "Абитуриент",
    // color: "secondary",
    // icon: <PersonIcon fontSize="small" />,
  },
};
