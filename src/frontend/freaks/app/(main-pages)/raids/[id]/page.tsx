import { BOSS_LABEL } from "@/domains/raids";
import {
  RaidLootService,
  RaidScreenshotsService,
  RaidsService,
  RaidParticipantsService,
} from "@/domains/raids/raids.service";
import { LootService } from "@/domains/loot";
import { UserService } from "@/domains/user/user.service";
import { requestServer } from "@/shared/api/serverRequest";
import { DateFormat, formatDate } from "@/utils/formateDate";
import { Metadata } from "next";
import BodyBlock from "./components/BodyBlock/BodyBlock";
import HeaderBlock from "./components/HeaderBlock/HeaderBlock";
import DetailContainer from "@/components/ui/DetailContainer/DetailContainer";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import NotFound from "@/components/ui/NotFound/NotFound";
import { auth } from "@/api/auth/auth";

type RaidPage = {
  params: Promise<{ id: string }>;
};

export async function generateMetadata({
  params,
}: RaidPage): Promise<Metadata> {
  const { id } = await params;

  const session = await auth();
  const accessToken = session?.accessToken;

  const raid = accessToken
    ? await requestServer(() =>
        RaidsService.getRaidByID(accessToken, Number(id))
      )
    : null;

  if (!raid) return { title: "Рейд не найден" };
  return {
    title: `${BOSS_LABEL[raid.bossType]} | ${formatDate(
      raid.startDt,
      DateFormat.SHORT_DATE_SHORT_YEAR_TIME
    )} `,
  };
}

export default async function Page({ params }: RaidPage) {
  const { id } = await params;
  const session = await auth();
  const accessToken = session?.accessToken;

  if (!accessToken) return <ErrorLoadData />;

  const raid = await requestServer(() =>
    RaidsService.getRaidByID(accessToken, Number(id))
  );

  if (!raid) return <NotFound />;

  const prefetchScreenshots =
    (await requestServer(() =>
      RaidScreenshotsService.getScreenshots(accessToken, Number(id))
    )) ?? [];

  const prefetchLoot =
    (await requestServer(() =>
      RaidLootService.getRaidLoot(accessToken, Number(id))
    )) ?? [];

  const prefetchLootItems =
    (await requestServer(() => LootService.getLootItems(accessToken))) ?? [];

  const prefetchParticipants =
    (await requestServer(() =>
      RaidParticipantsService.getParticipants(accessToken, Number(id))
    )) ?? [];

  const prefetchUsers =
    (await requestServer(() => UserService.getUsers(accessToken, true))) ?? [];

  return (
    <DetailContainer>
      <HeaderBlock raid={raid} />
      <BodyBlock
        // key={id}
        initialRaid={raid}
        prefetchScreenshots={prefetchScreenshots}
        prefetchLoot={prefetchLoot}
        prefetchLootItems={prefetchLootItems}
        prefetchParticipants={prefetchParticipants}
        prefetchUsers={prefetchUsers}
      />
    </DetailContainer>
  );
}
