"use client";

import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { SalaryGuildLeaderService } from "../../reports.service";
import type {
  CreateSalaryGuildLeaderRequest,
  SalaryGuildLeaderDto,
  UpdateSalaryGuildLeaderRequest,
} from "../../types";

type UpdateArgs = {
  salaryLootId: number;
  data: UpdateSalaryGuildLeaderRequest;
};

export function useSalaryGuildLeaderMutations(salaryId: number) {
  const {
    trigger: createGuildLeader,
    isMutating: isCreating,
    errorState: createErrorState,
  } = useProtectedSWRMutation<
    SalaryGuildLeaderDto,
    CreateSalaryGuildLeaderRequest
  >(`/salaries/${salaryId}/guild-leaders`, (token, data) =>
    SalaryGuildLeaderService.createGuildLeader(token, salaryId, data),
  );

  const {
    trigger: updateGuildLeader,
    isMutating: isUpdating,
    errorState: updateErrorState,
  } = useProtectedSWRMutation<SalaryGuildLeaderDto, UpdateArgs>(
    `/salaries/${salaryId}/guild-leaders/update`,
    (token, { salaryLootId, data }) =>
      SalaryGuildLeaderService.updateGuildLeader(
        token,
        salaryId,
        salaryLootId,
        data,
      ),
  );

  const {
    trigger: deleteGuildLeader,
    isMutating: isDeleting,
    errorState: deleteErrorState,
  } = useProtectedSWRMutation<void, number>(
    `/salaries/${salaryId}/guild-leaders/delete`,
    (token, salaryLootId) =>
      SalaryGuildLeaderService.deleteGuildLeader(token, salaryId, salaryLootId),
  );

  return {
    createGuildLeader,
    updateGuildLeader,
    deleteGuildLeader,
    isCreating,
    isUpdating,
    isDeleting,
    createErrorState,
    updateErrorState,
    deleteErrorState,
    isMutating: isCreating || isUpdating || isDeleting,
  };
}
