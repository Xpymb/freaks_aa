"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { SalaryLootService } from "../../reports.service";
import type { SalaryLootDto } from "../../types";

export function useGetSalaryLoots(salaryId?: number) {
  const { data, isLoading, errorState, mutate } = useProtectedSWR<
    SalaryLootDto[]
  >(salaryId ? `/salaries/${salaryId}/loots` : null, (token) =>
    SalaryLootService.getLoots(token, salaryId!),
  );

  return {
    loots: data ?? [],
    isLoading,
    errorState,
    refresh: mutate,
  };
}
