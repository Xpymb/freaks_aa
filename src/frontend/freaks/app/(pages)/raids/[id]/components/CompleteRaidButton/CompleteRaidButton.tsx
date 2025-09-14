"use client";

import React, { useState } from "react";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";
import { Check as CheckIcon } from "@mui/icons-material";
import { mutate } from "swr";
import { useRouter } from "next/navigation";
import { RaidItem, RaidStatus } from "@/domains/raids";
import { useHasRole } from "@/domains/auth/hooks/useHasRole";
import { useCompleteRaid } from "@/domains/raids/hooks/useCompleteRaid";
import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";

type Props = {
  raid: RaidItem;
  onRaidUpdated?: () => void;
};

const CompleteRaidButton = ({ raid, onRaidUpdated }: Props) => {
  const [open, setOpen] = useState(false);
  const router = useRouter();
  const { hasAccess } = useHasRole(['admin', 'guild_leader']);
  const { trigger: completeRaid, isMutating } = useCompleteRaid();

  const canComplete = hasAccess && raid.status !== RaidStatus.Ended;

  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const handleComplete = async () => {
    try {
      await completeRaid({ raidId: raid.id });
      handleClose();
      
      onRaidUpdated?.();
      
      await Promise.all([
        mutate(`/raids/${raid.id}`), // основные данные рейда
        mutate(`/raids/${raid.id}/screenshots`), // скриншоты
        mutate(`/raids/${raid.id}/loots`), // лут
        mutate(`/raids/${raid.id}/participants`), // участники
        mutate((key) => typeof key === "string" && key.startsWith("/raids")), // все запросы рейдов
      ]);
      
      // Переходим на список рейдов после успешного завершения
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
        <DialogTitle>
          Завершение рейда
        </DialogTitle>
        <DialogContent>
          <CustomTypography variant="body1">
            Вы уверены, что хотите завершить рейд {raid.id}?
          </CustomTypography>
          <CustomTypography variant="body2" className={styles.warningText}>
            Это действие нельзя отменить. Рейд будет переведен в статус &apos;Завершен&apos;.
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
