"use client";

import * as React from "react";
import {
  Checkbox,
  FormControl,
  FormControlLabel,
  FormGroup,
  FormHelperText,
  FormLabel,
} from "@mui/material";
import {
  Controller,
  type Control,
  type FieldPath,
  type FieldValues,
} from "react-hook-form";

export interface CheckboxOption<T = number> {
  value: T;
  label: string;
  disabled?: boolean;
}

type BaseProps<T = number> = {
  options: CheckboxOption<T>[];
  value: T[];
  onChange: (next: T[]) => void;
  label?: string;
  columns?: number;
  disabled?: boolean;
  error?: boolean;
  helperText?: string;
  className?: string;
};

function CheckboxGroupInner<T = number>({
  options,
  value,
  onChange,
  label,
  columns = 1,
  disabled,
  error,
  helperText,
  className,
}: BaseProps<T>) {
  const handleToggle = React.useCallback(
    (option: T) => {
      const next = value.includes(option)
        ? value.filter((v) => v !== option)
        : [...value, option];
      onChange(next);
    },
    [value, onChange],
  );

  return (
    <FormControl
      error={!!error}
      disabled={disabled}
      className={className}
      component="fieldset"
    >
      {label && <FormLabel component="legend">{label}</FormLabel>}
      <FormGroup
        sx={{
          display: "grid",
          gridTemplateColumns: `repeat(${columns}, 1fr)`,
        }}
      >
        {options.map((opt) => (
          <FormControlLabel
            key={String(opt.value)}
            label={opt.label}
            disabled={opt.disabled || disabled}
            control={
              <Checkbox
                checked={value.includes(opt.value)}
                onChange={() => handleToggle(opt.value)}
                size="small"
              />
            }
          />
        ))}
      </FormGroup>
      {helperText && <FormHelperText>{helperText}</FormHelperText>}
    </FormControl>
  );
}

export const CheckboxGroup = React.memo(CheckboxGroupInner) as <T = number>(
  p: BaseProps<T>,
) => React.JSX.Element;

/* ================= RHF wrapper ================= */

type FieldProps<FV extends FieldValues, T = number> = Omit<
  BaseProps<T>,
  "value" | "onChange" | "error" | "helperText"
> & {
  control: Control<FV>;
  name: FieldPath<FV>;
};

export function CheckboxGroupField<
  FV extends FieldValues,
  T = number,
>({ control, name, ...ui }: FieldProps<FV, T>) {
  return (
    <Controller
      control={control}
      name={name}
      render={({ field, fieldState }) => (
        <CheckboxGroup<T>
          {...ui}
          value={(field.value ?? []) as T[]}
          onChange={field.onChange}
          error={!!fieldState.error}
          helperText={fieldState.error?.message}
        />
      )}
    />
  );
}
