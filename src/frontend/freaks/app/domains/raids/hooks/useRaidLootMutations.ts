"use client";

import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { RaidLootService } from "../raids.service";
import type { CreateRaidLootRequest, UpdateRaidLootRequest } from "../types";

export function useRaidLootMutations(raidId: number) {
  const { trigger: createLoot, isMutating: isCreating } = useProtectedSWRMutation(
    `/raids/${raidId}/loots`,
    (token, { arg }: { arg: CreateRaidLootRequest }) => 
      RaidLootService.createRaidLoot(token, raidId, arg)
  );

  const { trigger: updateLoot, isMutating: isUpdating } = useProtectedSWRMutation(
    `/raids/${raidId}/loots/update`,
    (token, { arg }: { arg: { lootId: number; data: UpdateRaidLootRequest } }) => 
      RaidLootService.updateRaidLoot(token, raidId, arg.lootId, arg.data)
  );

  const { trigger: deleteLoot, isMutating: isDeleting } = useProtectedSWRMutation(
    `/raids/${raidId}/loots/delete`,
    (token, { arg }: { arg: { lootId: number } }) => 
      RaidLootService.deleteRaidLoot(token, raidId, arg.lootId)
  );

  return {
    createLoot,
    updateLoot,
    deleteLoot,
    isCreating,
    isUpdating,
    isDeleting,
    isMutating: isCreating || isUpdating || isDeleting,
  };
}
