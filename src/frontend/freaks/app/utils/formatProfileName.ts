import { IUser } from "@/domains/user/types";

export const formatProfileName = (profile: IUser | undefined) => {
  if (!profile) return "";

  return profile.gameNickname || profile.username || "Unknown";
};
