"use client";

import * as React from "react";
import { TextField, type TextFieldProps } from "@mui/material";
import {
  Controller,
  type Control,
  type FieldPath,
  type FieldValues,
  type ControllerProps,
} from "react-hook-form";

type BaseInputProps = {
  value: string;
  onChange: (next: string) => void;

  label?: string;
  placeholder?: string;
  size?: "small" | "medium";
  fullWidth?: boolean;
  type?: TextFieldProps["type"];
  disabled?: boolean;
  error?: boolean;
  helperText?: string;
  className?: string;
  id?: string;
  multiline?: boolean;
  rows?: number;
} & Omit<TextFieldProps, "value" | "onChange" | "error" | "helperText">;

function InputInner({
  value,
  onChange,
  label,
  placeholder,
  size = "small",
  fullWidth = true,
  type = "text",
  disabled,
  error,
  helperText,
  className,
  id,
  multiline,
  rows,
  ...rest
}: BaseInputProps) {
  const handleChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => onChange(e.target.value),
    [onChange]
  );

  return (
    <TextField
      id={id}
      className={className}
      value={value}
      onChange={handleChange}
      label={label}
      placeholder={placeholder}
      size={size}
      fullWidth={fullWidth}
      type={type}
      disabled={disabled}
      error={!!error}
      helperText={helperText}
      multiline={multiline}
      rows={rows}
      {...rest}
    />
  );
}

export const CustomInput = React.memo(InputInner) as (
  p: BaseInputProps
) => React.JSX.Element;

/* ================= RHF wrapper ================= */

type FieldInputProps<FV extends FieldValues> = Omit<
  BaseInputProps,
  "value" | "onChange" | "error" | "helperText"
> & {
  control: Control<FV>;
  name: FieldPath<FV>;
  rules?: ControllerProps<FV>["rules"];
  error?: boolean;
  helperText?: string;
};

export function InputField<FV extends FieldValues>({
  control,
  name,
  rules,
  error: errProp,
  helperText: helperProp,
  ...ui
}: FieldInputProps<FV>) {
  return (
    <Controller
      control={control}
      name={name}
      rules={rules}
      render={({ field, fieldState }) => (
        <CustomInput
          {...ui}
          value={(field.value ?? "") as string}
          onChange={field.onChange}
          error={errProp ?? !!fieldState.error}
          helperText={helperProp ?? fieldState.error?.message}
        />
      )}
    />
  );
}
