"use client";

import * as React from "react";
import { IconButton, Box, Typography } from "@mui/material";
import { useForm } from "react-hook-form";
import RestartAltIcon from "@mui/icons-material/RestartAlt";
import { BOSS_LABEL, RAID_STATUS_LABEL } from "@/domains/raids/constants";
import type { BossType, RaidStatus } from "@/domains/raids/types";
import type { RaidListQuery } from "@/domains/raids/raids.service";
import { AutoMultiField } from "@/components/ui/formInputs/CustomAutocomplete";
import { useDebounce } from "@/shared/hooks/useDebounce";
import styles from "./_styles.module.scss";
import DateOrRangeField from "@/components/ui/formInputs/DateField/DateField";
import { CustomTypography } from "@/components";

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

type Opt<T extends number> = { value: T; label: string };

const BOSS_OPTIONS: Opt<BossType>[] = Object.entries(BOSS_LABEL).map(
  ([v, l]) => ({ value: Number(v) as BossType, label: l })
);

const STATUS_OPTIONS: Opt<RaidStatus>[] = Object.entries(RAID_STATUS_LABEL).map(
  ([v, l]) => ({ value: Number(v) as RaidStatus, label: l })
);

const toISOorUndef = (v?: string) => {
  if (!v) return undefined;
  const d = new Date(v);
  return Number.isNaN(d.getTime()) ? undefined : d.toISOString().slice(0, 10);
};

export default function RaidsFilters({ initial, onApply, onReset }: Props) {
  const { control, reset, watch } = useForm<FormValues>({
    defaultValues: {
      bossTypes: initial?.BossTypes ?? [],
      statuses: initial?.Statuses ?? [],
      from: initial?.From ? initial.From : "",
      to: initial?.To ? initial.To : "",
    },
  });

  const watched = watch();
  const debounced = useDebounce<FormValues>(watched, 300);

  const first = React.useRef(true);
  const prevFilters = React.useRef<Partial<RaidListQuery>>({});

  React.useEffect(() => {
    if (first.current) {
      first.current = false;
      return;
    }

    const filters: Partial<RaidListQuery> = {
      BossTypes: debounced.bossTypes?.length ? debounced.bossTypes : undefined,
      Statuses: debounced.statuses?.length ? debounced.statuses : undefined,
      From: toISOorUndef(debounced.from),
      To: toISOorUndef(debounced.to),
      SortBy: initial?.SortBy,
      SortMode: initial?.SortMode,
    };

    // Проверяем, действительно ли фильтры изменились
    const filtersChanged =
      JSON.stringify(filters) !== JSON.stringify(prevFilters.current);

    if (filtersChanged) {
      prevFilters.current = filters;
      onApply(filters);
    }
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
    <Box className={styles.filtersPanel}>
      <Box className={styles.panelHeader}>
        <CustomTypography variant="h6" className={styles.panelTitle}>
          Фильтры рейдов
        </CustomTypography>
        <IconButton onClick={doReset} className={styles.resetButton}>
          <RestartAltIcon />
        </IconButton>
      </Box>

      <Box className={styles.panelContent}>
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
      </Box>
    </Box>
  );
}
