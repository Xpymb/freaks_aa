import { authorizedApi } from "@/shared/api/authorizedApi";

export const UserService = {
  getUser: (eduID: number, token: string) => authorizedApi(token).get(``),
};
