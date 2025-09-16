import { authorizedApi } from "@/shared/api/authorizedApi";
import { IUser } from "@/domains/user/types";

export interface UpdateUserRolesRequest {
  userId: string;
  userRoles: number[];
}

export const UserService = {
  getUser: (token: string) => authorizedApi(token, "portal").get(``),

  getUsers: (token: string, includeWoRoles = true) =>
    authorizedApi(token, "portal").get<IUser[]>(
      `/users?includeWoRoles=${includeWoRoles}`
    ),

  updateUserRoles: (token: string, data: UpdateUserRolesRequest) =>
    authorizedApi(token, "portal").patch<IUser>(`/users/${data.userId}/roles`, {
      userRoles: data.userRoles,
    }),

  getAllUsers: (token: string) =>
    authorizedApi(token, "portal").get<IUser[]>(`/users?includeWoRoles=true`),
};
