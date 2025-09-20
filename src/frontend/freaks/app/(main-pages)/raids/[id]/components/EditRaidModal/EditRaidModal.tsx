"use client";

import * as React from "react";
import { useForm } from "react-hook-form";
import { IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import CheckIcon from "@mui/icons-material/Check";

import styles from "./_styles.module.scss";

import CustomModal from "@/components/ui/CustomModal/CustomModal";
import { InputField } from "@/components/ui/formInputs/CustomInput";
import { SelectField } from "@/components/ui/formInputs/CustomSelect";
import { makeOptionsFromRecord } from "@/utils/makeOptionsFromRecord";
import { BOSS_LABEL, BossType, RaidItem } from "@/domains/raids";
import { UpdateRaidRequest, RaidsService } from "@/domains/raids/raids.service";
import { useAuth } from "@/store/authTokenStore";

type EditRaidForm = {
  bossType: BossType;
  description: string;
};

type Props = {
  open: boolean;
  onClose: () => void;
  raid: RaidItem;
  onSuccess?: () => void;
};

const EditRaidModal = ({ open, onClose, raid, onSuccess }: Props) => {
  const { accessToken } = useAuth();
  const [isUpdating, setIsUpdating] = React.useState(false);

  const { control, handleSubmit, reset } = useForm<EditRaidForm>({
    defaultValues: {
      bossType: raid.bossType,
      description: raid.description || "",
    },
  });

  React.useEffect(() => {
    reset({
      bossType: raid.bossType,
      description: raid.description || "",
    });
  }, [raid, reset]);

  const BOSS_OPTIONS = React.useMemo(
    () => makeOptionsFromRecord(BOSS_LABEL),
    []
  );

  const onSubmit = async (data: EditRaidForm) => {
    if (!accessToken) return;

    setIsUpdating(true);
    try {
      const updateData: UpdateRaidRequest = {
        bossType: Number(data.bossType),
        description: data.description,
      };

      await RaidsService.updateRaid(accessToken, raid.id, updateData);

      reset({
        bossType: data.bossType,
        description: data.description,
      });

      onSuccess?.();
      onClose();
    } catch (error) {
      console.error("Failed to update raid:", error);
    } finally {
      setIsUpdating(false);
    }
  };

  return (
    <CustomModal className={styles.modal} open={open} onClose={onClose}>
      <form className={styles.editRaidInputs} onSubmit={handleSubmit(onSubmit)}>
        <div className={styles.inputs}>
          <SelectField<EditRaidForm, BossType>
            className={styles.bossType}
            control={control}
            name="bossType"
            options={BOSS_OPTIONS}
            placeholder="Выберите босса"
            required
            rules={{
              required: "Необходимо выбрать тип босса",
            }}
          />

          <InputField<EditRaidForm>
            className={styles.description}
            control={control}
            name="description"
            label="Описание"
            multiline
            rows={3}
          />
        </div>

        <div className={styles.btnWrapper}>
          <IconButton type="submit" disabled={isUpdating}>
            <CheckIcon />
          </IconButton>
          <IconButton onClick={onClose} disabled={isUpdating}>
            <CloseIcon />
          </IconButton>
        </div>
      </form>
    </CustomModal>
  );
};

export default EditRaidModal;
