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
  if (filters.SortBy !== undefined) p.set("SortBy", String(filters.SortBy));
  if (filters.SortMode !== undefined)
    p.set("SortMode", String(filters.SortMode));
  if (filters.Take !== undefined) p.set("Take", String(filters.Take));
  if (filters.Skip !== undefined) p.set("Skip", String(filters.Skip));

  return p.toString();
}

export function useGetRaids(
  prefetchData: PaginatedList<RaidListItem> | null,
  filters?: Partial<RaidListQuery>
) {
  const query = buildRaidsQuery(filters);

  const key = `/raids${query ? `?${query}` : ""}`;

  const { data, isLoading, errorState, mutate } = useProtectedSWR<
    PaginatedList<RaidListItem>
  >(key, (token) => RaidsService.getRaids(token, query), {
    fallbackData: prefetchData || undefined,
    revalidateOnMount: false,
  });

  return {
    raids: data?.items ?? [],
    totalCount: data?.totalCount ?? 0,
    isLoading,
    errorState,
    refresh: () => mutate(),
  } as const;
}
