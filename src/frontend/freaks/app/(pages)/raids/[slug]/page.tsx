import { auth } from "@/api/auth/auth";
import { CustomTypography } from "@/components/ui/CustomTypography";
import { BOSS_LABEL } from "@/domains/raids";
import { RaidsService } from "@/domains/raids/raids.service";
import { requestServer } from "@/shared/api/serverRequest";
import { Metadata } from "next";

type RaidPage = {
  params: Promise<{ slug: string }>;
};

export async function generateMetadata({
  params,
}: RaidPage): Promise<Metadata> {
  const { slug } = await params;

  const session = await auth();
  const accessToken = session?.accessToken;

  const raid = accessToken
    ? await requestServer(() =>
        RaidsService.getRaidByID(Number(slug), accessToken)
      )
    : null;

  if (!raid) return { title: "Рейд не найден" };
  return {
    title: `${raid.startDt} | ${BOSS_LABEL[raid.bossType]}`,
  };
}

export default async function Page({ params }: RaidPage) {
  // const { slug } = await params;

  // const session = await auth();
  // const accessToken = session?.accessToken;

  // const raid = accessToken
  //   ? await requestServer(() =>
  //       RaidsService.getRaidByID(Number(slug), accessToken)
  //     )
  //   : null;

  // if (!raid) notFound();

  return (
    <CustomTypography variant="h1">
      {/* {BOSS_LABEL[raid.bossType]} */}
      dasdasdaa
    </CustomTypography>
  );
}
