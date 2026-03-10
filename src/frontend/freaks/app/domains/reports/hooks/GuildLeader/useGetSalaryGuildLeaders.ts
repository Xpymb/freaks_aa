"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { SalaryGuildLeaderService } from "../../reports.service";
import type { SalaryGuildLeaderDto } from "../../types";

export function useGetSalaryGuildLeaders(salaryId?: number) {
  const { data, isLoading, errorState, mutate } = useProtectedSWR<
    SalaryGuildLeaderDto[]
  >(salaryId ? `/salaries/${salaryId}/guild-leaders` : null, (token) =>
    SalaryGuildLeaderService.getGuildLeaders(token, salaryId!),
  );

  return {
    guildLeaders: data ?? [],
    isLoading,
    errorState,
    refresh: mutate,
  };
}
