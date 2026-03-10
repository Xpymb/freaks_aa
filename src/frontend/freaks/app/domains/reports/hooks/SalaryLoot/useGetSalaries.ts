"use client";

import { useMemo } from "react";
import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import type { PaginatedList } from "@/types/paginated.types";
import { type SalaryListQuery, SalaryService } from "../../reports.service";
import type { SalaryDto } from "../../types";

export function buildSalariesQuery(
  filters: Partial<SalaryListQuery> = {},
): string {
  const p = new URLSearchParams();

  if (filters.From) p.set("From", filters.From);
  if (filters.To) p.set("To", filters.To);
  if (filters.SortBy !== undefined) p.set("SortBy", String(filters.SortBy));
  if (filters.SortMode !== undefined)
    p.set("SortMode", String(filters.SortMode));
  if (filters.Take !== undefined) p.set("Take", String(filters.Take));
  if (filters.Skip !== undefined) p.set("Skip", String(filters.Skip));

  return p.toString();
}

export function useGetSalaries(filters?: Partial<SalaryListQuery>) {
  const query = useMemo(() => buildSalariesQuery(filters), [filters]);
  const key = useMemo(() => `/salaries${query ? `?${query}` : ""}`, [query]);

  const { data, isLoading, errorState, mutate } = useProtectedSWR<
    PaginatedList<SalaryDto>
  >(key, (token: string) => SalaryService.getSalaries(token, query), {
    revalidateOnMount: true,
  });

  return {
    salaries: data?.items ?? [],
    totalCount: data?.totalCount ?? 0,
    isLoading,
    errorState,
    refresh: () => mutate(),
  } as const;
}
