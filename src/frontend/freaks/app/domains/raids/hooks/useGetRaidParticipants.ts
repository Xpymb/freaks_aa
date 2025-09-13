import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { RaidParticipantDto } from "../types";
import { RaidParticipantsService } from "../raids.service";

export const useGetRaidParticipants = (
  fallbackData: RaidParticipantDto[],
  raidId: number
) => {
  return useProtectedSWR<RaidParticipantDto[]>(
    `/raids/${raidId}/participants`,
    (token) => RaidParticipantsService.getParticipants(token, raidId),
    {
      fallbackData,
      revalidateOnMount: false,
    }
  );
};
