"use client";

import * as React from "react";
import { useForm } from "react-hook-form";
import { IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import CheckIcon from "@mui/icons-material/Check";

import styles from "./_styles.module.scss";

import { useDisclosure } from "@/components/ui/useDisclosure";
import { CustomTypography } from "@/components/ui/CustomTypography";
import CustomModal from "@/components/ui/CustomModal/CustomModal";
import { InputField } from "@/components/ui/formInputs/CustomInput";
import { SelectField } from "@/components/ui/formInputs/CustomSelect";
import { makeOptionsFromRecord } from "@/utils/makeOptionsFromRecord";
import { BOSS_LABEL, BossType } from "@/domains/raids";
import { CreateRaidRequest, RaidsService } from "@/domains/raids/raids.service";
import { useTokens } from "@/store/authTokenStore";

const pad = (n: number) => String(n).padStart(2, "0");
const toLocalInput = (d: Date) =>
  `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(
    d.getHours()
  )}:${pad(d.getMinutes())}`;

type CreateRaidForm = {
  bossType: BossType | undefined;
  description: string;
  startDt: string;
};

export default function CreateRaid() {
  const { open, onOpen, onClose } = useDisclosure(false);

  const { accessToken } = useTokens();

  const {
    register,
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<CreateRaidForm>({
    defaultValues: {
      bossType: undefined,
      description: "",
      startDt: toLocalInput(new Date()),
    },
  });

  const BOSS_OPTIONS = React.useMemo(
    () => makeOptionsFromRecord<BossType>(BOSS_LABEL),
    []
  );

  const onSubmit = (data: CreateRaidForm) => {
    const payload: CreateRaidRequest = {
      bossType: Number(data.bossType!),
      description: data.description,
      startDt: new Date(data.startDt).toISOString(),
    };
    RaidsService.createRaid(accessToken!, payload);
    onClose();
  };

  return (
    <div className={styles.createRaid}>
      <div className={styles.createBtn} onClick={onOpen}>
        <CustomTypography variant="body2">Создать рейд</CustomTypography>
      </div>

      <CustomModal className={styles.modal} open={open} onClose={onClose}>
        <form
          className={styles.createRaidInputs}
          onSubmit={handleSubmit(onSubmit)}
        >
          <div className={styles.inputs}>
            <div className={styles.top}>
              <SelectField<CreateRaidForm, BossType>
                className={styles.bossType}
                control={control}
                name="bossType"
                // label="Тип босса"
                options={BOSS_OPTIONS}
                placeholder="Выберите босса"
                required
                rules={{
                  required: "Необходимо выбрать тип босса"
                }}
              />

              <InputField<CreateRaidForm>
                className={styles.date}
                control={control}
                name="startDt"
                label="Дата и время начала"
                type="datetime-local"
                required
                rules={{
                  required: "Необходимо указать дату и время начала"
                }}
              />
            </div>

            <InputField<CreateRaidForm>
              className={styles.description}
              control={control}
              name="description"
              label="Описание"
              multiline
              rows={3}
              required
              rules={{
                required: "Необходимо указать описание",
                minLength: {
                  value: 3,
                  message: "Описание должно содержать минимум 3 символа"
                }
              }}
            />
          </div>

          <div className={styles.btnWrapper}>
            <IconButton type="submit">
              <CheckIcon />
            </IconButton>
            <IconButton onClick={onClose}>
              <CloseIcon />
            </IconButton>
          </div>
        </form>
      </CustomModal>
    </div>
  );
}
