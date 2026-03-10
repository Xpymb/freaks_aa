"use client";

import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { SalaryLootService } from "../../reports.service";
import type {
  CreateSalaryLootRequest,
  SalaryLootDto,
  UpdateSalaryLootRequest,
} from "../../types";

type UpdateArgs = { lootId: number; data: UpdateSalaryLootRequest };

export function useSalaryLootMutations(salaryId: number) {
  const { trigger: createLoot, isMutating: isCreating, errorState: createError } =
    useProtectedSWRMutation<SalaryLootDto, CreateSalaryLootRequest>(
      `/salaries/${salaryId}/loots`,
      (token, data) => SalaryLootService.createLoot(token, salaryId, data),
    );

  const { trigger: updateLoot, isMutating: isUpdating } =
    useProtectedSWRMutation<SalaryLootDto, UpdateArgs>(
      `/salaries/${salaryId}/loots/update`,
      (token, { lootId, data }) =>
        SalaryLootService.updateLoot(token, salaryId, lootId, data),
    );

  const { trigger: deleteLoot, isMutating: isDeleting } =
    useProtectedSWRMutation<void, number>(
      `/salaries/${salaryId}/loots/delete`,
      (token, lootId) => SalaryLootService.deleteLoot(token, salaryId, lootId),
    );

  return {
    createLoot,
    updateLoot,
    deleteLoot,
    isCreating,
    isUpdating,
    isDeleting,
    isMutating: isCreating || isUpdating || isDeleting,
    createError,
  };
}
