"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { RaidListQuery, RaidsService } from "../raids.service";
import type { PaginatedList } from "@/types/paginated.types";
import type { RaidListItem } from "../types";

export function buildRaidsQuery(filters: Partial<RaidListQuery> = {}): string {
  const p = new URLSearchParams();

  filters.BossTypes?.forEach((v) => p.append("BossTypes", String(v)));
  filters.Statuses?.forEach((v) => p.append("Statuses", String(v)));

  if (filters.From) p.set("From", filters.From);
  if (filters.To) p.set("To", filters.To);

  return p.toString();
}

export function useGetRaids(filters?: Partial<RaidListQuery>) {
  const query = buildRaidsQuery(filters);

  const key = `/raids${query ? `?${query}` : ""}`;

  const { data, isLoading, errorState, mutate } = useProtectedSWR<
    PaginatedList<RaidListItem>
  >(key, (token) => RaidsService.getRaids(token, query));

  return {
    raids: data?.items ?? [],
    totalCount: data?.totalCount ?? 0,
    isLoading,
    errorState,
    refresh: () => mutate(),
  } as const;
}
