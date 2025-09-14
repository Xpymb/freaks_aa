"use client";

import * as React from "react";
import { useForm } from "react-hook-form";
import { IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import CheckIcon from "@mui/icons-material/Check";
import { useRouter } from "next/navigation";
import { mutate } from "swr";

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
  const router = useRouter();
  const { accessToken } = useTokens();
  const [isCreating, setIsCreating] = React.useState(false);

  const { control, handleSubmit } = useForm<CreateRaidForm>({
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

  const onSubmit = async (data: CreateRaidForm) => {
    setIsCreating(true);
    try {
      const payload: CreateRaidRequest = {
        bossType: Number(data.bossType!),
        description: data.description,
        startDt: new Date(data.startDt).toISOString(),
      };

      const createdRaid = await RaidsService.createRaid(accessToken!, payload);

      // Обновляем кеш списка рейдов
      await mutate(
        (key) => typeof key === "string" && key.startsWith("/raids")
      );

      onClose();

      // Переходим на страницу созданного рейда
      router.push(`/raids/${createdRaid.id}`);
    } catch (error) {
      console.error("Failed to create raid:", error);
      // Здесь можно добавить показ ошибки пользователю
    } finally {
      setIsCreating(false);
    }
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
                  required: "Необходимо выбрать тип босса",
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
                  required: "Необходимо указать дату и время начала",
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
            />
          </div>

          <div className={styles.btnWrapper}>
            <IconButton type="submit" disabled={isCreating}>
              <CheckIcon />
            </IconButton>
            <IconButton onClick={onClose} disabled={isCreating}>
              <CloseIcon />
            </IconButton>
          </div>
        </form>
      </CustomModal>
    </div>
  );
}
