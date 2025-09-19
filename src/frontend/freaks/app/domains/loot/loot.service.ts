import { authorizedApi } from "@/shared/api/authorizedApi";
import type { LootItemDto } from "./types";

export const LootService = {
  getLootItems: (token: string) =>
    authorizedApi(token, "portal").get<LootItemDto[]>("/loot-items"),
};
