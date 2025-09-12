import { authorizedApi } from "@/shared/api/authorizedApi";
import { IUser } from "@/types/user.types";

export const UserService = {
  getUser: (token: string) => authorizedApi(token, "portal").get(``),
  
  getUsers: (token: string, includeWoRoles = true) =>
    authorizedApi(token, "portal").get<IUser[]>(`/users?includeWoRoles=${includeWoRoles}`),
};
