import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { RaidParticipantDto } from "../types";
import { RaidParticipantsService } from "../raids.service";
import type {
  SSEMessage,
  RaidParticipantChangedMessage,
} from "@/types/sse.types";
import type { Key } from "swr";

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
      websocket: {
        channel: "raid.participant",
        enabled: true,
        onMessage: (data: SSEMessage, key: Key, mutate: () => void) => {
          const participantData = data?.pub
            ?.data as RaidParticipantChangedMessage;

          // Обновляем только если это изменение участников для текущего рейда
          if (participantData?.RaidId === raidId) {
            mutate();
          }
        },
      },
    }
  );
};
