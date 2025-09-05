"use client";

import * as React from "react";
import { useForm, Controller } from "react-hook-form";
import { TextField, IconButton, Button } from "@mui/material";
import AddCircleIcon from "@mui/icons-material/AddCircle";

import CloseIcon from "@mui/icons-material/Close";
import CheckIcon from "@mui/icons-material/Check";

import styles from "./_styles.module.scss";
import { AutoMultiField } from "@/components/ui/formInputs/CustomAutocomplete";
import { useDisclosure } from "@/components/ui/useDisclosure";

type CreateRaidForm = {
  bossType: number | null;
  description: string;
  startDt: string; // пока оставляем, позже добавим выбор даты
};

export default function CreateRaid() {
  const { open, onOpen, onClose } = useDisclosure(false);

  const { control, handleSubmit } = useForm<CreateRaidForm>({
    defaultValues: {
      bossType: null,
      description: "",
      startDt: new Date().toISOString(),
    },
  });

  const onSubmit = (data: CreateRaidForm) => {
    console.log("Создание рейда:", data);
    // TODO: RaidsService.postRaid(data)
  };

  return (
    <div className={styles.createRaid}>
      {!open && (
        <IconButton className={styles.openBtn} size="large" onClick={onOpen}>
          <AddCircleIcon />
        </IconButton>
      )}

      {open && (
        <form
          className={styles.createRaidInputs}
          onSubmit={handleSubmit(onSubmit)}
        >
          <div className={styles.inputs}>
            <AutoMultiField<CreateRaidForm, number>
              fullWidth
              control={control}
              name="bossType"
              label="Тип босса"
              options={[
                { value: 1, label: "Кракен" },
                { value: 2, label: "Левиафан" },
                { value: 3, label: "Анталон" },
              ]}
            />

            {/* Описание */}
            <Controller
              name="description"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Описание"
                  placeholder="Введите описание рейда"
                  fullWidth
                />
              )}
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
      )}
    </div>
  );
}
