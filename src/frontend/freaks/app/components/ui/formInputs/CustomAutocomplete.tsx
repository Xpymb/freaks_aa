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

export type Option<T extends string | number> = { value: T; label: string };

type BaseProps<T extends string | number> = {
  options: ReadonlyArray<Option<T>>;
  value: T[];
  onChange: (next: T[]) => void;

  label?: string;
  placeholder?: string;
  size?: "small" | "medium";
  fullWidth?: boolean;
  limitTags?: number;
  disabled?: boolean;
  error?: boolean;
  helperText?: string;
  className?: string;
  id?: string;
  disablePortal?: boolean;
};

function AutoMultiInner<T extends string | number>({
  options,
  value,
  onChange,
  label,
  placeholder,
  size = "small",
  fullWidth = true,
  limitTags,
  disabled,
  error,
  helperText,
  className,
  id,
  disablePortal,
}: BaseProps<T>) {
  const selected = React.useMemo(
    () => options.filter((o) => value.includes(o.value)),
    [options, value]
  );

  const handleChange = React.useCallback(
    (_: unknown, list: Option<T>[]) => onChange(list.map((o) => o.value)),
    [onChange]
  );

  return (
    <Autocomplete<Option<T>, true, false, false>
      id={id}
      className={className}
      multiple
      options={options as Option<T>[]}
      value={selected as Option<T>[]}
      onChange={handleChange}
      getOptionLabel={(o) => o.label}
      isOptionEqualToValue={(a, b) => a.value === b.value}
      disableCloseOnSelect
      filterSelectedOptions
      clearOnBlur={false}
      fullWidth={fullWidth}
      limitTags={limitTags}
      disabled={disabled}
      disablePortal={disablePortal}
      renderInput={(params) => (
        <TextField
          {...params}
          label={label}
          placeholder={placeholder}
          size={size}
          error={error}
          helperText={helperText}
        />
      )}
    />
  );
}

const CustomAutocomplete = React.memo(AutoMultiInner) as <
  T extends string | number
>(
  p: BaseProps<T>
) => JSX.Element;

export default CustomAutocomplete;

/* ================= RHF wrapper ================= */

type FieldProps<FV extends FieldValues, T extends string | number> = Omit<
  BaseProps<T>,
  "value" | "onChange"
> & {
  control: Control<FV>;
  name: FieldPath<FV>;
  rules?: ControllerProps<FV>["rules"];
};

export function AutoMultiField<
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
        <CustomAutocomplete<T>
          {...(restUi as Omit<BaseProps<T>, "value" | "onChange">)}
          value={(field.value ?? []) as T[]}
          onChange={field.onChange}
          error={errProp ?? !!fieldState.error}
          helperText={helperProp ?? fieldState.error?.message}
        />
      )}
    />
  );
}
