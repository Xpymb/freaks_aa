import { authorizedApi } from "@/shared/api/authorizedApi";
import { PaginatedList } from "@/types/paginated.types";
import {
  BossType,
  RaidFormatType,
  RaidItem,
  RaidListItem,
  RaidStatus,
} from "./types";

export type RaidListQuery = {
  BossTypes?: BossType[];
  Statuses?: RaidStatus[];
  From?: string;
  To?: string;
};

export interface CreateRaidRequest {
  bossType: BossType;
  startDt: string;
  description: string;
  format?: RaidFormatType;
}

export const RaidsService = {
  getRaids: (token: string, query = "") =>
    authorizedApi(token).get<PaginatedList<RaidListItem>>(
      `/raids${query ? `?${query}` : ""}`
    ),

  getRaidByID: (id: number, token: string) =>
    authorizedApi(token).get<RaidItem>(`/raids/${id}`),

  createRaid: (data: CreateRaidRequest, token: string) =>
    authorizedApi(token).post<RaidListItem>("/raids", data),
};
