import { authorizedApi } from "@/shared/api/authorizedApi";
import { PaginatedList } from "@/types/paginated.types";
import {
  CreateSalaryGuildLeaderRequest,
  CreateSalaryLootRequest,
  CreateSalaryRequest,
  SalaryDto,
  SalaryGuildLeaderDto,
  SalaryLootDto,
  UpdateSalaryGuildLeaderRequest,
  UpdateSalaryLootRequest,
  UpdateSalaryRequest,
} from "./types";

export type SalaryListQuery = {
  From?: string;
  To?: string;
  SortBy?: number;
  SortMode?: number;
  Take?: number;
  Skip?: number;
};

export const SalaryService = {
  openRegistration: (token: string, id: number) =>
    authorizedApi(token, "portal").post<SalaryDto>(
      `/salaries/${id}/open-registration`,
    ),
  closeRegistration: (token: string, id: number) =>
    authorizedApi(token, "portal").post<SalaryDto>(
      `/salaries/${id}/close-registration`,
    ),

  getSalaries: (token: string, query = "SortBy=0&SortMode=1&Take=10&Skip=0") =>
    authorizedApi(token, "portal").get<PaginatedList<SalaryDto>>(
      `/salaries${query ? `?${query}` : ""}`,
    ),

  getSalaryById: (token: string, id: number) =>
    authorizedApi(token, "portal").get<SalaryDto>(`/salaries/${id}`),

  createSalary: (token: string, data: CreateSalaryRequest) =>
    authorizedApi(token, "portal").post<SalaryDto>("/salaries", data),

  mutateSalary: (token: string, id: number, data: UpdateSalaryRequest) =>
    authorizedApi(token, "portal").put<SalaryDto>(`/salaries/${id}`, data),

  deleteSalary: (token: string, id: number) =>
    authorizedApi(token, "portal").delete<void>(`/salaries/${id}`),
};

export const SalaryLootService = {
  getLoots: (token: string, salaryId: number) =>
    authorizedApi(token, "portal").get<SalaryLootDto[]>(
      `/salaries/${salaryId}/loots`,
    ),

  createLoot: (
    token: string,
    salaryId: number,
    data: CreateSalaryLootRequest,
  ) =>
    authorizedApi(token, "portal").post<SalaryLootDto>(
      `/salaries/${salaryId}/loots`,
      data,
    ),

  updateLoot: (
    token: string,
    salaryId: number,
    lootId: number,
    data: UpdateSalaryLootRequest,
  ) =>
    authorizedApi(token, "portal").put<SalaryLootDto>(
      `/salaries/${salaryId}/loots/${lootId}`,
      data,
    ),

  deleteLoot: (token: string, salaryId: number, lootId: number) =>
    authorizedApi(token, "portal").delete<void>(
      `/salaries/${salaryId}/loots/${lootId}`,
    ),

  fillByRaids: (token: string, salaryId: number, lootIds: number[]) =>
    authorizedApi(token, "portal").post<SalaryLootDto[]>(
      `/salaries/${salaryId}/loots/fill-by-raids`,
      { lootIds },
    ),
};

export const SalaryGuildLeaderService = {
  getGuildLeaders: (token: string, salaryId: number) =>
    authorizedApi(token, "portal").get<SalaryGuildLeaderDto[]>(
      `/salaries/${salaryId}/guild-leaders`,
    ),
  createGuildLeader: (
    token: string,
    salaryId: number,
    data: CreateSalaryGuildLeaderRequest,
  ) =>
    authorizedApi(token, "portal").post<SalaryGuildLeaderDto>(
      `/salaries/${salaryId}/guild-leaders`,
      data,
    ),
  updateGuildLeader: (
    token: string,
    salaryId: number,
    salaryLootId: number,
    data: UpdateSalaryGuildLeaderRequest,
  ) =>
    authorizedApi(token, "portal").put<SalaryGuildLeaderDto>(
      `/salaries/${salaryId}/guild-leaders/${salaryLootId}`,
      data,
    ),
  deleteGuildLeader: (token: string, salaryId: number, salaryLootId: number) =>
    authorizedApi(token, "portal").delete<void>(
      `/salaries/${salaryId}/guild-leaders/${salaryLootId}`,
    ),
};
