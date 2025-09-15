"use client";

import React, { useState } from "react";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import { Check as CheckIcon } from "@mui/icons-material";
import { useRouter } from "next/navigation";
import { RaidItem, RaidStatus } from "@/domains/raids";
import { useCompleteRaid } from "@/domains/raids/hooks/useCompleteRaid";
import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";

type Props = {
  raid: RaidItem;
};

const CompleteRaidButton = ({ raid }: Props) => {
  const [open, setOpen] = useState(false);
  const router = useRouter();
  const { trigger: completeRaid, isMutating } = useCompleteRaid();

  const canComplete = raid.status !== RaidStatus.Ended;

  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const handleComplete = async () => {
    try {
      await completeRaid({ raidId: raid.id });
      handleClose();

      router.push("/raids");
    } catch (error) {
      console.error("Failed to complete raid:", error);
    }
  };

  if (!canComplete) {
    return null;
  }

  return (
    <>
      <Button
        variant="contained"
        color="success"
        startIcon={<CheckIcon />}
        onClick={handleOpen}
        disabled={isMutating}
        className={styles.completeButton}
      >
        Завершить рейд
      </Button>

      <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
        <DialogTitle>Завершение рейда</DialogTitle>
        <DialogContent>
          <CustomTypography variant="body1">
            Вы уверены, что хотите завершить рейд {raid.id}?
          </CustomTypography>
          <CustomTypography variant="body2" className={styles.warningText}>
            Это действие нельзя отменить. Рейд будет переведен в статус
            &apos;Завершен&apos;.
          </CustomTypography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} disabled={isMutating}>
            Отмена
          </Button>
          <Button
            onClick={handleComplete}
            variant="contained"
            color="success"
            disabled={isMutating}
            startIcon={<CheckIcon />}
          >
            {isMutating ? "Завершение..." : "Завершить рейд"}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default CompleteRaidButton;
