"use client";

import React from "react";
import {
  RaidItem,
  IRaidScreenshot,
  RaidLootDto,
  LootItemDto,
  RaidParticipantDto,
} from "@/domains/raids";
import { IUser } from "@/types/user.types";
import { useGetRaidByID } from "@/domains/raids/hooks/useGetRaidByID";
import HeaderBlock from "./HeaderBlock/HeaderBlock";
import BodyBlock from "./BodyBlock/BodyBlock";
import DetailContainer from "@/components/ui/DetailContainer/DetailContainer";

type Props = {
  initialRaid: RaidItem;
  prefetchScreenshots: IRaidScreenshot[];
  prefetchLoot: RaidLootDto[];
  prefetchLootItems: LootItemDto[];
  prefetchParticipants: RaidParticipantDto[];
  prefetchUsers: IUser[];
};

const RaidPageClient = ({
  initialRaid,
  prefetchScreenshots,
  prefetchLoot,
  prefetchLootItems,
  prefetchParticipants,
  prefetchUsers,
}: Props) => {
  const { raid } = useGetRaidByID(initialRaid, initialRaid.id);

  if (!raid) return null;

  return (
    <DetailContainer>
      <HeaderBlock raid={raid} />
      <BodyBlock
        raid={raid}
        prefetchScreenshots={prefetchScreenshots}
        prefetchLoot={prefetchLoot}
        prefetchLootItems={prefetchLootItems}
        prefetchParticipants={prefetchParticipants}
        prefetchUsers={prefetchUsers}
      />
    </DetailContainer>
  );
};

export default RaidPageClient;
