import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { IUser } from "@/types/user.types";
import { UserService } from "../user.service";

type GetUsersQuery = {
  fallbackData: IUser[];
  includeWoRoles: boolean;
};

export const useGetUsers = ({
  fallbackData = [],
  includeWoRoles,
}: GetUsersQuery) => {
  return useProtectedSWR<IUser[]>(
    `/users?includeWoRoles=${includeWoRoles}`,
    (token) => UserService.getUsers(token, true),
    {
      fallbackData,
      revalidateOnMount: false,
    }
  );
};
