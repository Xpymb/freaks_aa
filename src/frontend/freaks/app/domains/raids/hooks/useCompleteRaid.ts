import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { RaidsService } from "../raids.service";
import { RaidItem } from "../types";

export const useCompleteRaid = () => {
  return useProtectedSWRMutation<RaidItem, { raidId: number }>(
    "complete-raid",
    (token, { raidId }) => RaidsService.completeRaid(token, raidId)
  );
};
