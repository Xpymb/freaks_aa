"use client";

import * as React from "react";
import { FormControlLabel, Switch } from "@mui/material";
import {
  Controller,
  type Control,
  type FieldPath,
  type FieldValues,
} from "react-hook-form";

type BaseProps = {
  checked: boolean;
  onChange: (next: boolean) => void;
  label?: string;
  disabled?: boolean;
  className?: string;
};

function SwitchInner({
  checked,
  onChange,
  label,
  disabled,
  className,
}: BaseProps) {
  const handleChange = React.useCallback(
    (_: React.ChangeEvent<HTMLInputElement>, val: boolean) => onChange(val),
    [onChange],
  );

  return (
    <FormControlLabel
      className={className}
      disabled={disabled}
      label={label ?? ""}
      control={<Switch checked={checked} onChange={handleChange} />}
    />
  );
}

export const CustomSwitch = React.memo(SwitchInner);

/* ================= RHF wrapper ================= */

type FieldProps<FV extends FieldValues> = Omit<
  BaseProps,
  "checked" | "onChange"
> & {
  control: Control<FV>;
  name: FieldPath<FV>;
};

export function SwitchField<FV extends FieldValues>({
  control,
  name,
  ...ui
}: FieldProps<FV>) {
  return (
    <Controller
      control={control}
      name={name}
      render={({ field }) => (
        <CustomSwitch
          {...ui}
          checked={!!field.value}
          onChange={field.onChange}
        />
      )}
    />
  );
}
