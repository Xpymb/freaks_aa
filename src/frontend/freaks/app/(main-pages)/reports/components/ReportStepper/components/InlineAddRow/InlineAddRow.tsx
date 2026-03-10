"use client";

import React from "react";
import { IconButton } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import CheckIcon from "@mui/icons-material/Check";
import CloseIcon from "@mui/icons-material/Close";
import { type GridApi } from "@mui/x-data-grid";
import SingleAutocomplete, {
  type Option,
} from "@/components/ui/formInputs/SingleAutocomplete";
import { CustomInput } from "@/components/ui/formInputs/CustomInput";
import { useDisclosure } from "@/components/ui/useDisclosure";
import { useColumnWidths } from "./hooks/useColumnWidths";
import styles from "./_styles.module.scss";

// ─── типы полей ───────────────────────────────────────────────────────────────

type NumberFieldDef = {
  field: string;
  type: "number";
  placeholder?: string;
};

type AutocompleteFieldDef = {
  field: string;
  type: "autocomplete";
  placeholder?: string;
  options: Option<number>[];
  loading?: boolean;
  noOptionsText?: string;
  renderOption?: (
    props: React.HTMLAttributes<HTMLLIElement>,
    option: Option<number>,
  ) => React.ReactNode;
  renderStartAdornment?: (option: Option<number>) => React.ReactNode;
};

type DisplayFieldDef<T extends InlineAddRowValues> = {
  field: string;
  type: "display";
  render: (values: T) => React.ReactNode;
};

export type AddRowFieldDef<T extends InlineAddRowValues = InlineAddRowValues> =
  | NumberFieldDef
  | AutocompleteFieldDef
  | DisplayFieldDef<T>;

// ─── типы ─────────────────────────────────────────────────────────────────────

export type InlineAddRowValues = Record<string, string | number | null>;

type Props<T extends InlineAddRowValues> = {
  fields: AddRowFieldDef<T>[];
  values: T;
  onChange: (patch: Partial<T>) => void;
  onSave: () => void;
  onCancel: () => void;
  apiRef: React.RefObject<GridApi | null>;
  isSaving?: boolean;
  disabled?: boolean;
  openLabel?: string;
  requiredField?: string;
  validate?: (values: T) => boolean;
  errorMessage?: string;
};

// ─── компонент ────────────────────────────────────────────────────────────────

export function InlineAddRow<T extends InlineAddRowValues>({
  fields,
  values,
  onChange,
  onSave,
  onCancel,
  apiRef,
  isSaving,
  disabled,
  openLabel = "ДОБАВИТЬ СТРОКУ",
  requiredField,
  validate,
  errorMessage,
}: Props<T>) {
  const { open: isOpen, onOpen, onClose } = useDisclosure();
  const columnWidths = useColumnWidths(apiRef);

  const gridTemplateColumns = columnWidths.length
    ? columnWidths.map((w) => `${w}px`).join(" ")
    : `repeat(${fields.length + 1}, 1fr)`;

  const canSave =
    (requiredField ? Boolean(values[requiredField]) : true) &&
    (validate ? validate(values) : true);

  const handleCancel = () => {
    onClose();
    onCancel();
  };

  const handleSave = () => {
    onClose();
    onSave();
  };

  if (!isOpen) {
    return (
      <button
        className={styles.addButton}
        onClick={onOpen}
        disabled={disabled}
        type="button"
      >
        <AddIcon fontSize="small" className={styles.addButtonIcon} />
        {openLabel}
      </button>
    );
  }

  return (
    <div className={styles.wrapper}>
      {errorMessage && (
        <div className={styles.errorMessage}>{errorMessage}</div>
      )}
    <div className={styles.row} style={{ gridTemplateColumns }}>
      {fields.map((fieldDef) => {
        if (fieldDef.type === "autocomplete") {
          return (
            <div key={fieldDef.field} className={styles.cell}>
              <SingleAutocomplete<number>
                options={fieldDef.options}
                value={(values[fieldDef.field] as number | null) ?? null}
                onChange={(v) =>
                  onChange({ [fieldDef.field]: v } as Partial<T>)
                }
                placeholder={fieldDef.placeholder}
                loading={fieldDef.loading}
                noOptionsText={fieldDef.noOptionsText}
                size="small"
                renderOption={fieldDef.renderOption}
                renderStartAdornment={fieldDef.renderStartAdornment}
              />
            </div>
          );
        }

        if (fieldDef.type === "display") {
          return (
            <div key={fieldDef.field} className={`${styles.cell} ${styles.cellDisplay}`}>
              {fieldDef.render(values)}
            </div>
          );
        }

        return (
          <div key={fieldDef.field} className={`${styles.cell} ${styles.cellNumber}`}>
            <CustomInput
              type="number"
              placeholder={fieldDef.placeholder}
              value={(values[fieldDef.field] as string) ?? ""}
              onChange={(v) => onChange({ [fieldDef.field]: v } as Partial<T>)}
              size="small"
              fullWidth
            />
          </div>
        );
      })}

      <div className={styles.actionsCell}>
        <IconButton
          size="small"
          onClick={handleSave}
          disabled={!canSave || isSaving}
          color="success"
        >
          <CheckIcon fontSize="small" />
        </IconButton>
        <IconButton size="small" onClick={handleCancel}>
          <CloseIcon fontSize="small" />
        </IconButton>
      </div>
    </div>
    </div>
  );
}
