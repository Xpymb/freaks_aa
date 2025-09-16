import { SiteMetaData } from "@/enum";
import styles from "./_styles.module.scss";
import { CustomContainer } from "@/components/ui/CustomContainer";
import { CustomTypography } from "@/components/ui/CustomTypography";
import { auth } from "@/api/auth/auth";
import { requestServer } from "@/shared/api/serverRequest";
import { RaidsService } from "@/domains/raids";
import RaidList from "./components/RaidList/RaidList";

export async function generateMetadata() {
  return {
    title: `${SiteMetaData.title} | Home`,
    description: `${SiteMetaData.description}`,
  };
}

export default async function Page() {
  const session = await auth();
  const accessToken = session?.accessToken;

  const raids = accessToken
    ? await requestServer(() => RaidsService.getRaids(accessToken))
    : null;

  return (
    <section className={styles.raidListSection}>
      <CustomContainer maxWidth="lg">
        <div className={styles.heading}>
          <CustomTypography
            fontWeight={600}
            variant="h1"
            className={styles.title}
          >
            Рейды
          </CustomTypography>
          <CustomTypography variant="h5" className={styles.subtitle}>
            Создание, редактирование и управление рейдами гильдии
          </CustomTypography>
        </div>

        <RaidList prefetchRaids={raids ?? undefined} />
      </CustomContainer>
    </section>
  );
}
