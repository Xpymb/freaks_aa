"use client";

import React, { useState } from "react";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import { Delete as DeleteIcon } from "@mui/icons-material";
import { useRouter } from "next/navigation";
import { RaidItem, useDeleteRaid, useRaidPermissions } from "@/domains/raids";
import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";

type Props = {
  raid: RaidItem;
};

const DeleteRaidButton = ({ raid }: Props) => {
  const [open, setOpen] = useState(false);
  const router = useRouter();
  const { trigger: deleteRaid, isMutating } = useDeleteRaid();
  const { canDelete } = useRaidPermissions(raid);

  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const handleDelete = async () => {
    try {
      await deleteRaid({ raidId: raid.id });
      handleClose();

      router.push("/raids");
    } catch (error) {
      console.error("Failed to delete raid:", error);
    }
  };

  if (!canDelete) {
    return null;
  }

  return (
    <>
      <Button
        variant="contained"
        color="error"
        startIcon={<DeleteIcon />}
        onClick={handleOpen}
        disabled={isMutating}
        className={styles.deleteButton}
      >
        Удалить рейд
      </Button>

      <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
        <DialogTitle>Удаление рейда</DialogTitle>
        <DialogContent>
          <CustomTypography variant="body1">
            Вы уверены, что хотите удалить рейд {raid.id}?
          </CustomTypography>
          <CustomTypography variant="body2" className={styles.warningText}>
            Это действие нельзя отменить. Все данные рейда, включая участников,
            лут и скриншоты, будут безвозвратно удалены.
          </CustomTypography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} disabled={isMutating}>
            Отмена
          </Button>
          <Button
            onClick={handleDelete}
            variant="contained"
            color="error"
            disabled={isMutating}
            startIcon={<DeleteIcon />}
          >
            {isMutating ? "Удаление..." : "Удалить рейд"}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default DeleteRaidButton;
