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
import { motion } from "framer-motion";

type Props = {
  raid: RaidItem;
};

const HeaderBlock = ({ raid }: Props) => {
  const containerVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: {
      opacity: 1,
      y: 0,
      transition: {
        duration: 0.6,
        ease: "easeOut" as const,
        staggerChildren: 0.1,
      },
    },
  };

  const itemVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: {
      opacity: 1,
      y: 0,
      transition: {
        duration: 0.5,
        ease: "easeOut" as const,
      },
    },
  };

  return (
    <motion.section
      className={styles.raidHeaderSection}
      variants={containerVariants}
      initial="hidden"
      animate="visible"
    >
      <div className={styles.wrapper}>
        <motion.div className={styles.top} variants={itemVariants}>
          <Link href="/raids">
            <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
              <IconButton className={styles.returnIcon}>
                <ReplyIcon />
              </IconButton>
            </motion.div>
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
        </motion.div>
        <motion.div className={styles.bottom} variants={itemVariants}>
          <div className={styles.infoWrapper}>
            <motion.div
              className={styles.infoBlock}
              whileHover={{ scale: 1.02 }}
              transition={{ duration: 0.2 }}
            >
              <CustomTypography className={styles.muted} variant="subtitle1">
                Создатель:
              </CustomTypography>
              <CustomTypography variant="subtitle1">
                {raid.creator.gameNickname}
              </CustomTypography>
            </motion.div>

            <Divider orientation="vertical" flexItem />

            <motion.div
              className={styles.infoBlock}
              whileHover={{ scale: 1.02 }}
              transition={{ duration: 0.2 }}
            >
              <CustomTypography className={styles.muted} variant="subtitle1">
                Начало:
              </CustomTypography>
              <CustomTypography variant="subtitle1">
                {formatDate(
                  raid.startDt,
                  DateFormat.SHORT_DATE_SHORT_YEAR_TIME
                )}
              </CustomTypography>
            </motion.div>

            <Divider orientation="vertical" flexItem />

            <motion.div
              className={styles.infoBlock}
              whileHover={{ scale: 1.02 }}
              transition={{ duration: 0.2 }}
            >
              <CustomTypography className={styles.muted} variant="subtitle1">
                Дата создания:
              </CustomTypography>
              <CustomTypography variant="subtitle1">
                {formatDate(
                  raid.createdDt,
                  DateFormat.SHORT_DATE_SHORT_YEAR_TIME
                )}
              </CustomTypography>
            </motion.div>
            {raid.updatedDt && (
              <>
                <Divider orientation="vertical" flexItem />

                <motion.div
                  className={styles.infoBlock}
                  whileHover={{ scale: 1.02 }}
                  transition={{ duration: 0.2 }}
                >
                  <CustomTypography
                    className={styles.muted}
                    variant="subtitle1"
                  >
                    Обновлено:
                  </CustomTypography>
                  <CustomTypography variant="subtitle1">
                    {raid.updatedDt}
                  </CustomTypography>
                </motion.div>
              </>
            )}
          </div>

          <RaidConditionalRender raid={raid} permission="canManage">
            <motion.div
              className={styles.buttonsContainer}
              variants={itemVariants}
            >
              <RaidConditionalRender raid={raid} permission="canComplete">
                <CompleteRaidButton raid={raid} />
              </RaidConditionalRender>

              <RaidConditionalRender raid={raid} permission="canDelete">
                <DeleteRaidButton raid={raid} />
              </RaidConditionalRender>
            </motion.div>
          </RaidConditionalRender>
        </motion.div>
      </div>
    </motion.section>
  );
};

export default HeaderBlock;
