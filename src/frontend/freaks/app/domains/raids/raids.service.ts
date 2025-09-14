import { authorizedApi } from "@/shared/api/authorizedApi";
import { PaginatedList } from "@/types/paginated.types";
import {
  BossType,
  CreateRaidLootRequest,
  CreateRaidParticipantRequest,
  IRaidScreenshot,
  LootItemDto,
  RaidItem,
  RaidListItem,
  RaidLootDto,
  RaidParticipantDto,
  RaidStatus,
  UpdateRaidLootRequest,
} from "./types";

export type RaidListQuery = {
  BossTypes?: BossType[];
  Statuses?: RaidStatus[];
  From?: string;
  To?: string;
  SortBy?: number;
  SortMode?: number;
  Take?: number;
  Skip?: number;
};

export interface CreateRaidRequest {
  bossType: number;
  startDt: string;
  description: string;
}

export interface CreateScreenshotRequest {
  screenshotUris: string[];
}

export const RaidsService = {
  getRaids: (token: string, query = "") =>
    authorizedApi(token, "portal").get<PaginatedList<RaidListItem>>(
      `/raids${query ? `?${query}` : ""}`
    ),

  getRaidByID: (token: string, id: number) =>
    authorizedApi(token, "portal").get<RaidItem>(`/raids/${id}`),

  createRaid: (token: string, data: CreateRaidRequest) =>
    authorizedApi(token, "portal").post<RaidListItem>("/raids", data),

  completeRaid: (token: string, raidId: number) =>
    authorizedApi(token, "portal").post<RaidItem>(`/raids/${raidId}/finish`),
};

export const RaidScreenshotsService = {
  getScreenshots: (token: string, raidId: number) =>
    authorizedApi(token, "portal").get<IRaidScreenshot[]>(
      `/raids/${raidId}/screenshots`
    ),

  postScreenshot: (
    token: string,
    raidId: number,
    data: CreateScreenshotRequest
  ) =>
    authorizedApi(token, "portal").post(`/raids/${raidId}/screenshots`, data),

  deleteScreenshotByUrl: (token: string, raidId: number, url: string) =>
    authorizedApi(token, "portal").delete<void>(
      `/raids/${raidId}/screenshots?screenshotUrl=${url}`
    ),
};

export const RaidLootService = {
  getRaidLoot: (token: string, raidId: number) =>
    authorizedApi(token, "portal").get<RaidLootDto[]>(
      `/raids/${raidId}/loots`
    ),

  createRaidLoot: (
    token: string,
    raidId: number,
    data: CreateRaidLootRequest
  ) =>
    authorizedApi(token, "portal").post<RaidLootDto>(
      `/raids/${raidId}/loots`,
      data
    ),

  updateRaidLoot: (
    token: string,
    raidId: number,
    lootId: number,
    data: UpdateRaidLootRequest
  ) =>
    authorizedApi(token, "portal").put<RaidLootDto>(
      `/raids/${raidId}/loots/${lootId}`,
      data
    ),

  deleteRaidLoot: (token: string, raidId: number, lootId: number) =>
    authorizedApi(token, "portal").delete<void>(
      `/raids/${raidId}/loots/${lootId}`
    ),
};

export const LootItemsService = {
  getLootItems: (token: string) =>
    authorizedApi(token, "portal").get<LootItemDto[]>("/loot-items"),
};

export const RaidParticipantsService = {
  getParticipants: (token: string, raidId: number) =>
    authorizedApi(token, "portal").get<RaidParticipantDto[]>(
      `/raids/${raidId}/participants`
    ),

  createParticipant: (
    token: string,
    raidId: number,
    data: CreateRaidParticipantRequest
  ) =>
    authorizedApi(token, "portal").post<RaidParticipantDto>(
      `/raids/${raidId}/participants`,
      data
    ),

  deleteParticipant: (token: string, raidId: number, participantId: string) =>
    authorizedApi(token, "portal").delete<void>(
      `/raids/${raidId}/participants/${participantId}`
    ),
};
