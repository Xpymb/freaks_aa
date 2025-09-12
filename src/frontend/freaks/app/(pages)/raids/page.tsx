import { SiteMetaData } from "@/enum";
import styles from "./_styles.module.scss";
import RaidList from "./componets/RaidList/RaidList";
import { CustomContainer } from "@/components/ui/CustomContainer";
import { CustomTypography } from "@/components/ui/CustomTypography";

export async function generateMetadata() {
  return {
    title: `${SiteMetaData.title} | Home`,
    description: `${SiteMetaData.description}`,
  };
}

export default function Page() {
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

        <RaidList />
      </CustomContainer>
    </section>
  );
}
