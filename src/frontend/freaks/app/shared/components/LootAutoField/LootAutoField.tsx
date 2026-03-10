"use client";

import React from "react";
import {
  type Control,
  type ControllerProps,
  type FieldPath,
  type FieldValues,
} from "react-hook-form";
import {
  type Option,
  SingleAutoField,
} from "@/components/ui/formInputs/SingleAutocomplete";
import type { LootItemDto } from "@/domains/loot/types";
import CustomImage from "@/components/ui/CustomImage";
import LootItemOption from "@/shared/components/LootAutoField/components/LootItemOption/LootItemOption";
import styles from "./_styles.module.scss";

export const renderLootStartAdornment = (option: Option<number>) => {
  const item = option.data as LootItemDto | undefined;
  if (!item?.iconUri) return null;
  return (
    <div className={styles.adornment}>
      <CustomImage
        src={`${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${item.iconUri}`}
        alt={item.name}
        fill
        className={styles.adornmentIcon}
      />
      <CustomImage
        src={`/images/masks/icon_grade${item.gradeType - 1}.png`}
        alt=""
        fill
        className={styles.adornmentMask}
      />
    </div>
  );
};

type Props<FV extends FieldValues> = {
  control: Control<FV>;
  name: FieldPath<FV>;
  items: LootItemDto[];
  rules?: ControllerProps<FV>["rules"];
  label?: string;
  placeholder?: string;
  disabled?: boolean;
  loading?: boolean;
  noOptionsText?: string;
};

export function LootAutoField<FV extends FieldValues>({
  control,
  name,
  items,
  rules,
  label,
  placeholder = "Предмет",
  disabled,
  loading,
  noOptionsText = "Предметы не найдены",
}: Props<FV>) {
  const options: Option<number>[] = items.map((item) => ({
    value: item.id,
    label: item.name,
    data: item,
  }));

  return (
    <SingleAutoField<FV, number>
      control={control}
      name={name}
      rules={rules}
      options={options}
      label={label}
      placeholder={placeholder}
      disabled={disabled}
      loading={loading}
      noOptionsText={noOptionsText}
      renderOption={(props, option) => (
        <LootItemOption key={option.value} props={props} option={option} />
      )}
      renderStartAdornment={renderLootStartAdornment}
    />
  );
}
