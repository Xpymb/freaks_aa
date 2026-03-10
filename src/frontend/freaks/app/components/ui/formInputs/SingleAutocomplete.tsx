"use client";

import * as React from "react";
import { Autocomplete, TextField } from "@mui/material";
import {
  Controller,
  type Control,
  type FieldPath,
  type FieldValues,
  type ControllerProps,
} from "react-hook-form";
import { JSX } from "react";

export type Option<T extends string | number> = { 
  value: T; 
  label: string; 
  data?: unknown; // Дополнительные данные для кастомного рендеринга
};

type BaseProps<T extends string | number> = {
  options: ReadonlyArray<Option<T>>;
  value: T | null;
  onChange: (next: T | null) => void;

  label?: string;
  placeholder?: string;
  size?: "small" | "medium";
  fullWidth?: boolean;
  disabled?: boolean;
  error?: boolean;
  helperText?: string;
  className?: string;
  id?: string;
  disablePortal?: boolean;
  loading?: boolean;
  loadingText?: string;
  noOptionsText?: string;
  renderOption?: (props: React.HTMLAttributes<HTMLLIElement>, option: Option<T>) => React.ReactNode;
  renderStartAdornment?: (option: Option<T>) => React.ReactNode;
};

function SingleAutoInner<T extends string | number>({
  options,
  value,
  onChange,
  label,
  placeholder,
  size = "small",
  fullWidth = true,
  disabled,
  error,
  helperText,
  className,
  id,
  disablePortal,
  loading,
  loadingText = "Загрузка...",
  noOptionsText = "Нет вариантов",
  renderOption,
  renderStartAdornment,
}: BaseProps<T>) {
  const selected = React.useMemo(
    () => options.find((o) => o.value === value) || null,
    [options, value]
  );

  const handleChange = React.useCallback(
    (_: unknown, option: Option<T> | null) => onChange(option?.value || null),
    [onChange]
  );

  return (
    <Autocomplete<Option<T>, false, false, false>
      id={id}
      className={className}
      options={options as Option<T>[]}
      value={selected}
      onChange={handleChange}
      getOptionLabel={(o) => o.label}
      getOptionKey={(o) => o.value}
      isOptionEqualToValue={(a, b) => a.value === b.value}
      fullWidth={fullWidth}
      disabled={disabled}
      disablePortal={disablePortal}
      loading={loading}
      loadingText={loadingText}
      noOptionsText={noOptionsText}
      renderOption={renderOption}
      renderInput={(params) => (
        <TextField
          {...params}
          label={label}
          placeholder={placeholder}
          size={size}
          error={error}
          helperText={helperText}
          InputProps={{
            ...params.InputProps,
            startAdornment: selected && renderStartAdornment
              ? renderStartAdornment(selected)
              : params.InputProps.startAdornment,
          }}
        />
      )}
    />
  );
}

const SingleAutocomplete = React.memo(SingleAutoInner) as <
  T extends string | number
>(
  p: BaseProps<T>
) => JSX.Element;

export default SingleAutocomplete;

/* ================= RHF wrapper ================= */

type FieldProps<FV extends FieldValues, T extends string | number> = Omit<
  BaseProps<T>,
  "value" | "onChange"
> & {
  control: Control<FV>;
  name: FieldPath<FV>;
  rules?: ControllerProps<FV>["rules"];
};

export function SingleAutoField<
  FV extends FieldValues,
  T extends string | number
>({ control, name, rules, ...ui }: FieldProps<FV, T>) {
  const { error: errProp, helperText: helperProp, ...restUi } = ui;
  return (
    <Controller
      control={control}
      name={name}
      rules={rules}
      render={({ field, fieldState }) => (
        <SingleAutocomplete<T>
          {...(restUi as Omit<BaseProps<T>, "value" | "onChange">)}
          value={(field.value ?? null) as T | null}
          onChange={field.onChange}
          error={errProp ?? !!fieldState.error}
          helperText={helperProp ?? fieldState.error?.message}
        />
      )}
    />
  );
}
