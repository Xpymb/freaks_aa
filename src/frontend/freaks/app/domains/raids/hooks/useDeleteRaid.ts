import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { RaidsService } from "../raids.service";

export const useDeleteRaid = () => {
  return useProtectedSWRMutation<void, { raidId: number }>(
    "delete-raid",
    (token, { raidId }) => RaidsService.deleteRaid(token, raidId)
  );
};
