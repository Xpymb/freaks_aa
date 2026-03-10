"use client";

import { SalaryService } from "@/domains/reports";
import { SalaryDto } from "@/domains/reports/types";
import { useProtectedSWR } from "@/shared/api/useProtectedSWR";

export function useGetSalaryById(id?: number) {
  const { data, isLoading, errorState, mutate } = useProtectedSWR<SalaryDto>(
    id ? `/salaries/${id}` : null,
    (token) => SalaryService.getSalaryById(token, id!),
  );

  return {
    salary: data,
    isLoading,
    errorState,
    refresh: () => mutate(),
  };
}
