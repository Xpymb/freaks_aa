import { BOSS_LABEL, RaidItem } from "@/domains/raids";
import styles from "./_styles.module.scss";
import { CustomTypography } from "@/components/ui/CustomTypography";
import StatusChip from "@/(main-pages)/raids/components/StatusChip/StatusChip";
import { Divider, IconButton, Link } from "@mui/material";
import { DateFormat, formatDate } from "@/utils/formateDate";
import CompleteRaidButton from "../CompleteRaidButton/CompleteRaidButton";
import DeleteRaidButton from "../DeleteRaidButton/DeleteRaidButton";
import { RaidConditionalRender } from "@/components/ui";
import ReplyIcon from "@mui/icons-material/Reply";
import { HelpHint } from "@/components/ui/HelpHint/HelpHint";

type Props = {
  raid: RaidItem;
};

const HeaderBlock = ({ raid }: Props) => {
  return (
    <section className={styles.raidHeaderSection}>
      <div className={styles.wrapper}>
        <div className={styles.top}>
          <Link href="/raids">
            <div>
              <IconButton className={styles.returnIcon}>
                <ReplyIcon />
              </IconButton>
            </div>
          </Link>

          <div className={styles.topLeft}>
            <CustomTypography variant="caption" className={styles.muted}>
              Raid.ID: {raid.id}
            </CustomTypography>
            <CustomTypography variant="h1">
              {BOSS_LABEL[raid.bossType]}
            </CustomTypography>
          </div>

          <div className={styles.topRight}>
            <StatusChip status={raid.status} />
          </div>
        </div>
        <div className={styles.bottom}>
          <div className={styles.infoWrapper}>
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
                <HelpHint title="Дата указывается в вашем локальном часовом поясе" />
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
                <HelpHint title="Дата указывается в вашем локальном часовом поясе" />
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
                    {formatDate(
                      raid.updatedDt,
                      DateFormat.SHORT_DATE_SHORT_YEAR_TIME
                    )}
                    <HelpHint title="Дата указывается в вашем локальном часовом поясе" />
                  </CustomTypography>
                </div>
              </>
            )}
          </div>

          <RaidConditionalRender raid={raid} permission="canManage">
            <div className={styles.buttonsContainer}>
              <RaidConditionalRender raid={raid} permission="canComplete">
                <CompleteRaidButton raid={raid} />
              </RaidConditionalRender>

              <RaidConditionalRender raid={raid} permission="canDelete">
                <DeleteRaidButton raid={raid} />
              </RaidConditionalRender>
            </div>
          </RaidConditionalRender>
        </div>
      </div>
    </section>
  );
};

export default HeaderBlock;
