"use client";

import * as React from "react";
import { IconButton } from "@mui/material";
import { useForm } from "react-hook-form";
import RestartAltIcon from "@mui/icons-material/RestartAlt";
import { BOSS_LABEL, RAID_STATUS_LABEL } from "@/domains/raids/constants";
import type { BossType, RaidStatus } from "@/domains/raids/types";
import type { RaidListQuery } from "@/domains/raids/raids.service";
import { AutoMultiField } from "@/components/ui/formInputs/CustomAutocomplete";
import { useDebounce } from "@/shared/hooks/useDebounce";
import styles from "./_styles.module.scss";
import DateOrRangeField from "@/components/ui/formInputs/DateField/DateField";

type FormValues = {
  bossTypes: BossType[];
  statuses: RaidStatus[];
  from?: string;
  to?: string;
};

type Props = {
  initial?: Partial<RaidListQuery>;
  onApply: (filters: Partial<RaidListQuery>) => void;
  onReset?: () => void;
};

const toDatetimeLocal = (iso?: string) => {
  if (!iso) return "";
  const d = new Date(iso);
  const pad = (n: number) => String(n).padStart(2, "0");
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(
    d.getHours()
  )}:${pad(d.getMinutes())}`;
};

const toISO = (local?: string) =>
  local ? new Date(local).toISOString() : undefined;

type Opt<T extends number> = { value: T; label: string };

const BOSS_OPTIONS: Opt<BossType>[] = Object.entries(BOSS_LABEL).map(
  ([v, l]) => ({ value: Number(v) as BossType, label: l })
);

const STATUS_OPTIONS: Opt<RaidStatus>[] = Object.entries(RAID_STATUS_LABEL).map(
  ([v, l]) => ({ value: Number(v) as RaidStatus, label: l })
);

export default function RaidsFilters({ initial, onApply, onReset }: Props) {
  const {
    control,
    register,
    reset,
    watch,
    formState: { errors },
  } = useForm<FormValues>({
    defaultValues: {
      bossTypes: initial?.BossTypes ?? [],
      statuses: initial?.Statuses ?? [],
      from: initial?.From ? toDatetimeLocal(initial.From) : "",
      to: initial?.To ? toDatetimeLocal(initial.To) : "",
    },
  });

  const watched = watch();
  const debounced = useDebounce<FormValues>(watched, 300);

  const first = React.useRef(true);
  React.useEffect(() => {
    if (first.current) {
      first.current = false;
      return;
    }

    const filters: Partial<RaidListQuery> = {
      BossTypes: debounced.bossTypes?.length ? debounced.bossTypes : undefined,
      Statuses: debounced.statuses?.length ? debounced.statuses : undefined,
      From: toISO(debounced.from),
      To: toISO(debounced.to),
      // Сохраняем параметры сортировки из initial
      SortBy: initial?.SortBy,
      SortMode: initial?.SortMode,
    };

    onApply(filters);
  }, [debounced, onApply, initial?.SortBy, initial?.SortMode]);

  const doReset = () => {
    reset({
      bossTypes: [],
      statuses: [],
      from: "",
      to: "",
    });
    onApply({
      // Сохраняем параметры сортировки при сбросе
      SortBy: initial?.SortBy,
      SortMode: initial?.SortMode,
    });
    onReset?.();
  };

  return (
    <div className={styles.filters}>
      <div className={styles.wrapper}>
        <AutoMultiField<FormValues, BossType>
          control={control}
          name="bossTypes"
          options={BOSS_OPTIONS}
          label="Тип босса"
          limitTags={3}
        />

        <AutoMultiField<FormValues, RaidStatus>
          control={control}
          name="statuses"
          options={STATUS_OPTIONS}
          label="Статус"
          limitTags={3}
        />

        <DateOrRangeField
          control={control}
          nameFrom="from"
          nameTo="to"
          label="Период"
        />

        <IconButton onClick={doReset}>
          <RestartAltIcon />
        </IconButton>
      </div>
    </div>
  );
}
