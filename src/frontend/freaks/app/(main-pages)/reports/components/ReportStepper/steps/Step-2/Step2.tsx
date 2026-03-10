"use client";

import React, { useCallback, useMemo, useState } from "react";
import { GridColDef, useGridApiRef } from "@mui/x-data-grid";
import { IconButton, Tooltip } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import { CustomDataGrid } from "@/components";
import { useGetLootItems } from "@/domains/loot/hooks/useGetLootItems";
import { useGetSalaryLoots } from "@/domains/reports/hooks/SalaryLoot/useGetSalaryLoots";
import { useSalaryLootMutations } from "@/domains/reports/hooks/SalaryLoot/useSalaryLootMutations";
import type { SalaryLootDto } from "@/domains/reports/types";
import LootItemOption from "@/shared/components/LootAutoField/components/LootItemOption/LootItemOption";
import LootItemCell from "../../components/LootItemCell/LootItemCell";
import {
  type AddRowFieldDef,
  InlineAddRow,
} from "../../components/InlineAddRow/InlineAddRow";
import { renderLootStartAdornment } from "@/shared/components/LootAutoField/LootAutoField";
import type { StepProps } from "../../ReportStepper";
import styles from "./_styles.module.scss";
import Image from "next/image";
import TitleBlock from "@/(main-pages)/reports/components/ReportStepper/components/TitleBlock/TitleBlock";

type NewRowState = {
  lootItemId: number | null;
  quantity: string;
  pricePerItem: string;
  discountPercent: string;
};

const defaultNewRow: NewRowState = {
  lootItemId: null,
  quantity: "",
  pricePerItem: "",
  discountPercent: "",
};

const computeAmount = (q: number, p: number, d: number) =>
  Math.round(q * p * (1 - d / 100));

const Step2 = ({ salaryId }: StepProps) => {
  const apiRef = useGridApiRef();
  const { loots, isLoading, refresh } = useGetSalaryLoots(salaryId);
  const { createLoot, deleteLoot, isCreating, createError } =
    useSalaryLootMutations(salaryId!);

  const { lootItems, isLoading: lootItemsLoading } = useGetLootItems();

  const [newRow, setNewRow] = useState<NewRowState>(defaultNewRow);

  const total = loots.reduce((sum, r) => sum + r.amount, 0);

  const lootOptions = lootItems.map((item) => ({
    value: item.id,
    label: item.name,
    data: item,
  }));

  const handleSaveNewRow = async () => {
    if (!newRow.lootItemId || !salaryId) return;
    const quantity = parseFloat(newRow.quantity) || 0;
    const pricePerItem = parseFloat(newRow.pricePerItem) || 0;
    if (quantity <= 0 || pricePerItem <= 0) return;
    const discountPercent = parseFloat(newRow.discountPercent) || 0;
    await createLoot({
      lootId: newRow.lootItemId,
      quantity,
      pricePerItem,
      discountPercent,
      amount: computeAmount(quantity, pricePerItem, discountPercent),
    });
    void refresh();
    setNewRow(defaultNewRow);
  };

  const handleDelete = useCallback(
    async (lootId: number) => {
      if (!salaryId) return;
      await deleteLoot(lootId);
      void refresh();
    },
    [salaryId, deleteLoot, refresh],
  );

  const columns: GridColDef<SalaryLootDto>[] = useMemo(
    () => [
      {
        field: "lootItem",
        headerName: "Предмет",
        flex: 1.5,
        renderCell: (params) => <LootItemCell item={params.value} />,
      },
      { field: "quantity", headerName: "Кол-во", flex: 0.8, type: "number" },
      {
        field: "pricePerItem",
        renderHeader: () => (
          <span style={{ display: "flex", alignItems: "center", gap: 4 }}>
            Цена за 1 ед
            <Image
              src="/icons/money_icon.svg"
              alt="gold"
              width={14}
              height={14}
            />
          </span>
        ),
        flex: 1.2,
        type: "number",
        renderCell: (params) => (
          <span className={styles.amountCell}>
            {params.value?.toLocaleString()}
          </span>
        ),
      },
      {
        field: "discountPercent",
        headerName: "Скидка, %",
        flex: 0.8,
        type: "number",
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
              onClick={() => handleDelete(params.row.id)}
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

  const fields: AddRowFieldDef<NewRowState>[] = [
    {
      field: "lootItemId",
      type: "autocomplete",
      placeholder: "Предмет",
      options: lootOptions,
      loading: lootItemsLoading,
      noOptionsText: "Предметы не найдены",
      renderOption: (props, option) => (
        <LootItemOption key={option.value} props={props} option={option} />
      ),
      renderStartAdornment: renderLootStartAdornment,
    },
    { field: "quantity", type: "number", placeholder: "Кол-во" },
    { field: "pricePerItem", type: "number", placeholder: "Цена" },
    { field: "discountPercent", type: "number", placeholder: "Скидка %" },
    {
      field: "amount",
      type: "display",
      render: (values) => {
        const q = parseFloat(values.quantity as string) || 0;
        const p = parseFloat(values.pricePerItem as string) || 0;
        const d = parseFloat(values.discountPercent as string) || 0;
        const amount = values.lootItemId ? computeAmount(q, p, d) : null;
        return amount !== null ? amount.toLocaleString() : "—";
      },
    },
  ];

  return (
    <section className={styles.wrapper}>
      <TitleBlock
        title="Продано за период"
        subtitle="Тут указывается лут полученный гильдией"
      />

      <div className={styles.tableArea}>
        <CustomDataGrid
          rows={loots}
          columns={columns}
          loading={isLoading}
          apiRef={apiRef}
          fillHeight
          footer={
            <InlineAddRow<NewRowState>
              fields={fields}
              values={newRow}
              apiRef={apiRef}
              isSaving={isCreating}
              disabled={!salaryId}
              requiredField="lootItemId"
              validate={(v) =>
                parseFloat(v.quantity as string) > 0 &&
                parseFloat(v.pricePerItem as string) > 0
              }
              onChange={(patch) => setNewRow((p) => ({ ...p, ...patch }))}
              onSave={handleSaveNewRow}
              onCancel={() => setNewRow(defaultNewRow)}
              errorMessage={
                createError.isError
                  ? (createError.message ?? undefined)
                  : undefined
              }
            />
          }
        />
      </div>

      <div className={styles.totalRow}>
        <span className={styles.totalLabel}>Итого золота</span>
        <span className={styles.totalValue}>{total.toLocaleString()}</span>
      </div>
    </section>
  );
};

export default Step2;
