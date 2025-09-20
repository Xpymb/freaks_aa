"use client";

import { BOSS_LABEL, RaidItem } from "@/domains/raids";
import { useGetRaidByID } from "@/domains/raids/hooks/useGetRaidByID";
import styles from "./_styles.module.scss";
import { CustomTypography } from "@/components/ui/CustomTypography";
import StatusChip from "@/(main-pages)/raids/components/StatusChip/StatusChip";
import { Divider, IconButton, Link } from "@mui/material";
import { DateFormat, formatDate } from "@/utils/formateDate";
import CompleteRaidButton from "../CompleteRaidButton/CompleteRaidButton";
import DeleteRaidButton from "../DeleteRaidButton/DeleteRaidButton";
import { RaidConditionalRender } from "@/components/ui";
import ReplyIcon from "@mui/icons-material/Reply";
import EditIcon from "@mui/icons-material/Edit";
import { HelpHint } from "@/components/ui/HelpHint/HelpHint";
import EditRaidModal from "../EditRaidModal/EditRaidModal";
import { useDisclosure } from "@/components/ui/useDisclosure";

type Props = {
  raid: RaidItem;
};

const HeaderBlock = ({ raid: initialRaid }: Props) => {
  const { raid } = useGetRaidByID(initialRaid, initialRaid.id);
  const {
    open: isEditModalOpen,
    onOpen: handleEditClick,
    onClose: handleEditClose,
  } = useDisclosure();

  const handleEditSuccess = () => {
    // Данные обновятся автоматически через SSE
    handleEditClose();
  };

  // Используем данные из SWR или fallback на initialRaid
  const currentRaid = raid || initialRaid;

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
              Raid.ID: {currentRaid.id}
            </CustomTypography>
            <div className={styles.bossTitleContainer}>
              <CustomTypography variant="h1">
                {BOSS_LABEL[currentRaid.bossType]}
              </CustomTypography>
              <RaidConditionalRender
                raid={currentRaid}
                permission="canEditInfo"
              >
                <IconButton
                  onClick={handleEditClick}
                  className={styles.editButton}
                  size="small"
                >
                  <EditIcon />
                </IconButton>
              </RaidConditionalRender>
            </div>
          </div>

          <div className={styles.topRight}>
            <StatusChip status={currentRaid.status} />
          </div>
        </div>
        <div className={styles.bottom}>
          <div className={styles.infoWrapper}>
            <div className={styles.infoBlock}>
              <CustomTypography className={styles.muted} variant="subtitle1">
                Создатель:
              </CustomTypography>
              <CustomTypography variant="subtitle1">
                {currentRaid.creator.gameNickname}
              </CustomTypography>
            </div>

            <Divider orientation="vertical" flexItem />

            <div className={styles.infoBlock}>
              <CustomTypography className={styles.muted} variant="subtitle1">
                Начало:
              </CustomTypography>
              <CustomTypography variant="subtitle1">
                {formatDate(
                  currentRaid.startDt,
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
                  currentRaid.createdDt,
                  DateFormat.SHORT_DATE_SHORT_YEAR_TIME
                )}
                <HelpHint title="Дата указывается в вашем локальном часовом поясе" />
              </CustomTypography>
            </div>
            {currentRaid.updatedDt && (
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
                      currentRaid.updatedDt,
                      DateFormat.SHORT_DATE_SHORT_YEAR_TIME
                    )}
                    <HelpHint title="Дата указывается в вашем локальном часовом поясе" />
                  </CustomTypography>
                </div>
              </>
            )}
          </div>

          <RaidConditionalRender raid={currentRaid} permission="canManage">
            <div className={styles.buttonsContainer}>
              <RaidConditionalRender
                raid={currentRaid}
                permission="canComplete"
              >
                <CompleteRaidButton raid={currentRaid} />
              </RaidConditionalRender>

              <RaidConditionalRender raid={currentRaid} permission="canDelete">
                <DeleteRaidButton raid={currentRaid} />
              </RaidConditionalRender>
            </div>
          </RaidConditionalRender>
        </div>
      </div>

      <EditRaidModal
        open={isEditModalOpen}
        onClose={handleEditClose}
        raid={currentRaid}
        onSuccess={handleEditSuccess}
      />
    </section>
  );
};

export default HeaderBlock;
