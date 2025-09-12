import { IUser } from "@/types/user.types";

export const formatProfileName = (profile: IUser | undefined) => {
  if (!profile) return "";

  return profile.gameNickname || profile.username || "Unknown";
};
