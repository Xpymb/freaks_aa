"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@mui/material";
import { Add } from "@mui/icons-material";
import { CustomTypography } from "@/components/ui/CustomTypography";
import { NumberInputField } from "@/components/ui/NumberInput/NumberInput";
import { LootItemDto } from "@/domains/loot";
import { useRaidLootMutations } from "@/domains/raids/hooks/useRaidLootMutations";
import { type AddLootFormData, addLootSchema } from "./addLootSchema";
import { LootAutoField } from "@/shared/components/LootAutoField/LootAutoField";
import styles from "./_styles.module.scss";
import React from "react";

type Props = {
  raidId: number;
  onLootAdded: () => void;
  prefetchLootItems: LootItemDto[];
};

const AddLootForm = ({ raidId, onLootAdded, prefetchLootItems }: Props) => {
  const { createLoot, isCreating } = useRaidLootMutations(raidId);

  const {
    control,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<AddLootFormData>({
    resolver: zodResolver(addLootSchema),
    defaultValues: {
      quantity: 1,
    },
  });

  const onSubmit = async (data: AddLootFormData) => {
    await createLoot({ arg: { lootId: data.lootId, quantity: data.quantity } });
    reset();
    onLootAdded();
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className={styles.addLootForm}>
        <div className={styles.formHeader}>
          <CustomTypography variant="h3">Добавить лут</CustomTypography>
        </div>

        <div className={styles.formContent}>
          {Object.keys(errors).length > 0 && (
            <div className={styles.formErrors}>
              {errors.lootId && (
                <div className={styles.errorMessage}>
                  {errors.lootId.message}
                </div>
              )}
              {errors.quantity && (
                <div className={styles.errorMessage}>
                  {errors.quantity.message}
                </div>
              )}
            </div>
          )}

          <div className={styles.formRow}>
            <div className={styles.autocompleteWrapper}>
              <LootAutoField<AddLootFormData>
                control={control}
                name="lootId"
                items={prefetchLootItems}
                label="Выберите предмет"
                placeholder="Начните вводить название..."
                disabled={isCreating}
              />
            </div>

            <div className={styles.quantityWrapper}>
              <NumberInputField<AddLootFormData>
                control={control}
                name="quantity"
                min={1}
                max={999}
                label="Количество"
                disabled={isCreating}
              />
            </div>

            <Button
              type="submit"
              variant="contained"
              disabled={isCreating}
              startIcon={<Add />}
              className={styles.addButton}
            >
              {isCreating ? "Добавление..." : "Добавить"}
            </Button>
          </div>
        </div>
      </div>
    </form>
  );
};

export default AddLootForm;
