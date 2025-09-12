import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { RaidParticipantsService } from "../raids.service";
import { CreateRaidParticipantRequest, RaidParticipantDto } from "../types";

export const useRaidParticipantMutations = (raidId: number) => {
  const createParticipant = useProtectedSWRMutation<
    RaidParticipantDto,
    CreateRaidParticipantRequest
  >(
    `create-participant-${raidId}`,
    (token, data) => RaidParticipantsService.createParticipant(token, raidId, data),
    {
      onSuccess: () => {
        // Invalidate participants list
        window.dispatchEvent(new CustomEvent('refresh-participants'));
      },
    }
  );

  const deleteParticipant = useProtectedSWRMutation<void, string>(
    `delete-participant-${raidId}`,
    (token, participantId) => RaidParticipantsService.deleteParticipant(token, raidId, participantId),
    {
      onSuccess: () => {
        // Invalidate participants list
        window.dispatchEvent(new CustomEvent('refresh-participants'));
      },
    }
  );

  return {
    createParticipant,
    deleteParticipant,
  };
};
