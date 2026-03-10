"use client";

import React, { useCallback, useMemo } from "react";
import { GridColDef, useGridApiRef } from "@mui/x-data-grid";
import { IconButton, Tooltip } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import Image from "next/image";
import { CustomDataGrid } from "@/components";
import TitleBlock from "@/(main-pages)/reports/components/ReportStepper/components/TitleBlock/TitleBlock";
import LootItemCell from "@/(main-pages)/reports/components/ReportStepper/components/LootItemCell/LootItemCell";
import { useGetSalaryGuildLeaders } from "@/domains/reports/hooks/GuildLeader/useGetSalaryGuildLeaders";
import { useSalaryGuildLeaderMutations } from "@/domains/reports/hooks/GuildLeader/useSalaryGuildLeaderMutations";
import type { SalaryGuildLeaderDto } from "@/domains/reports";
import type { StepProps } from "../../ReportStepper";
import styles from "./_styles.module.scss";

const Step3 = ({ salaryId }: StepProps) => {
  const apiRef = useGridApiRef();
  const { guildLeaders, isLoading, refresh } = useGetSalaryGuildLeaders(salaryId);
  const { deleteGuildLeader } = useSalaryGuildLeaderMutations(salaryId!);

  const handleDelete = useCallback(
    async (salaryLootId: number) => {
      if (!salaryId) return;
      await deleteGuildLeader(salaryLootId);
      void refresh();
    },
    [salaryId, deleteGuildLeader, refresh],
  );

  const columns: GridColDef<SalaryGuildLeaderDto>[] = useMemo(
    () => [
      {
        field: "salaryLoot",
        headerName: "Предмет",
        flex: 1.5,
        renderCell: (params) => <LootItemCell item={params.value?.lootItem} />,
      },
      {
        field: "quantity",
        headerName: "Кол-во",
        flex: 0.8,
        type: "number",
      },
      {
        field: "pricePerItem",
        renderHeader: () => (
          <span style={{ display: "flex", alignItems: "center", gap: 4 }}>
            Цена за 1 ед
            <Image src="/icons/money_icon.svg" alt="gold" width={14} height={14} />
          </span>
        ),
        flex: 1.2,
        type: "number",
        valueGetter: (_value, row) => row.salaryLoot?.pricePerItem,
        renderCell: (params) => (
          <span className={styles.amountCell}>
            {params.value?.toLocaleString()}
          </span>
        ),
      },
      {
        field: "amount",
        headerName: "Сумма",
        flex: 1,
        type: "number",
        renderCell: (params) => (
          <span className={styles.amountCell}>
            {params.value?.toLocaleString()}
          </span>
        ),
      },
      {
        field: "actions",
        headerName: "",
        width: 60,
        sortable: false,
        renderCell: (params) => (
          <Tooltip title="Удалить" placement="left" arrow>
            <IconButton
              size="small"
              onClick={() => handleDelete(params.row.salaryLoot.id)}
              className={styles.deleteButton}
            >
              <DeleteIcon fontSize="small" />
            </IconButton>
          </Tooltip>
        ),
      },
    ],
    [handleDelete],
  );

  return (
    <section className={styles.wrapper}>
      <TitleBlock
        title="Доля руководства"
        subtitle="Предметы для расчёта доли гильд-лидеров"
      />

      <div className={styles.tableArea}>
        <CustomDataGrid
          rows={guildLeaders}
          columns={columns}
          loading={isLoading}
          apiRef={apiRef}
          fillHeight
          getRowId={(row) => row.salaryLoot.id}
        />
      </div>
    </section>
  );
};

export default Step3;
