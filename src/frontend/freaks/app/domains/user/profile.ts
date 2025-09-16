import { IUser } from "@/domains/user/types";

export type UserRole = "student" | "employee" | "entrant";

export interface UserProfile extends Omit<IUser, "roles"> {
  roles: UserRole[];
}

export const useProfile = () => {
  // TODO: Implement profile hook
  return {
    profile: null as UserProfile | null,
    isLoading: false,
  };
};
