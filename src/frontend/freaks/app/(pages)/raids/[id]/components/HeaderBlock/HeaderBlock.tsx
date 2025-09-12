import { BOSS_LABEL, RaidItem } from "@/domains/raids";
import styles from "./_styles.module.scss";
import { CustomTypography } from "@/components/ui/CustomTypography";
import StatusChip from "@/(pages)/raids/componets/StatusChip/StatusChip";
import { CustomContainer } from "@/components/ui/CustomContainer";
import { Divider } from "@mui/material";
import { DateFormat, formatDate } from "@/utils/formateDate";

type Props = {
  raid: RaidItem;
};

const HeaderBlock = ({ raid }: Props) => {
  return (
    <section className={styles.raidHeaderSection}>
      <CustomContainer maxWidth="lg">
        <div className={styles.wrapper}>
          <div className={styles.top}>
            <CustomTypography variant="body1">
              Raid.ID: {raid.id}
            </CustomTypography>
            <StatusChip status={raid.status} />
          </div>
          <div className={styles.middle}>
            <CustomTypography variant="h1">
              {BOSS_LABEL[raid.bossType]}
            </CustomTypography>
          </div>
          <div className={styles.bottom}>
            <div className={styles.infoBlock}>
              <CustomTypography className={styles.muted} variant="subtitle1">
                Создатель:
              </CustomTypography>
              <CustomTypography variant="subtitle1">
                {raid.creator.gameNickname}
              </CustomTypography>
            </div>

            <Divider orientation="vertical" flexItem />

            <div className={styles.infoBlock}>
              <CustomTypography className={styles.muted} variant="subtitle1">
                Начало:
              </CustomTypography>
              <CustomTypography variant="subtitle1">
                {formatDate(
                  raid.startDt,
                  DateFormat.SHORT_DATE_SHORT_YEAR_TIME
                )}
              </CustomTypography>
            </div>

            <Divider orientation="vertical" flexItem />

            <div className={styles.infoBlock}>
              <CustomTypography className={styles.muted} variant="subtitle1">
                Дата создания:
              </CustomTypography>
              <CustomTypography variant="subtitle1">
                {formatDate(
                  raid.createdDt,
                  DateFormat.SHORT_DATE_SHORT_YEAR_TIME
                )}
              </CustomTypography>
            </div>
            {raid.updatedDt && (
              <>
                <Divider orientation="vertical" flexItem />

                <div className={styles.infoBlock}>
                  <CustomTypography
                    className={styles.muted}
                    variant="subtitle1"
                  >
                    Обновлено:
                  </CustomTypography>
                  <CustomTypography variant="subtitle1">
                    {raid.updatedDt}
                  </CustomTypography>
                </div>
              </>
            )}
          </div>
        </div>
      </CustomContainer>
    </section>
  );
};

export default HeaderBlock;
