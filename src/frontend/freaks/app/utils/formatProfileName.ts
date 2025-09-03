import { IProfile } from "@/domains/user/profile/types";

export const formatProfileName = (profile: IProfile | undefined) => {
  if (!profile) return "";

  const { lastName, firstName, patronymic } = profile;

  return [
    lastName,
    firstName?.[0] && `${firstName[0]}.`,
    patronymic?.[0] && `${patronymic[0]}.`,
  ]
    .filter(Boolean)
    .join(" ");
};
