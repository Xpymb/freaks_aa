"use client";

import * as React from "react";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
import FormHelperText from "@mui/material/FormHelperText";
import type { SelectChangeEvent, SelectProps } from "@mui/material/Select";
import {
  Controller,
  type Control,
  type FieldPath,
  type FieldValues,
  type ControllerProps,
} from "react-hook-form";

// Option type for select items
export interface SelectOption<T = string | number> {
  value: T;
  label: React.ReactNode;
  disabled?: boolean;
}

// Props for the custom select wrapper
export interface CustomSelectProps<T = string | number> {
  id?: string;
  label?: string;
  options: SelectOption<T>[];
  value: T | "";
  onChange: (value: T | "") => void;
  placeholder?: string;
  error?: boolean;
  helperText?: React.ReactNode;
  disabled?: boolean;
  required?: boolean;
  fullWidth?: boolean;
  size?: "small" | "medium";
  variant?: "outlined" | "filled" | "standard";
  className?: string;
}

// Custom MUI Select wrapper component
function CustomSelectComponent<T = string | number>({
  id,
  label,
  options,
  value,
  onChange,
  placeholder,
  error = false,
  helperText,
  disabled = false,
  required = false,
  fullWidth = true,
  size = "small",
  variant = "outlined",
  className,
  ...rest
}: CustomSelectProps<T> & Omit<SelectProps, "value" | "onChange" | "error">) {
  const labelId = id ? `${id}-label` : undefined;

  const handleChange = React.useCallback(
    (event: SelectChangeEvent<unknown>) => {
      const selectedValue = event.target.value as string;

      if (selectedValue === "") {
        onChange("");
        return;
      }

      // Find the option that matches the selected value
      const foundOption = options.find(
        (option) => String(option.value) === selectedValue
      );

      if (foundOption) {
        onChange(foundOption.value);
      } else {
        onChange("");
      }
    },
    [onChange, options]
  );

  const renderValue = React.useCallback(
    (selected: unknown) => {
      if (selected === "" || selected == null) {
        return placeholder || "";
      }

      const foundOption = options.find(
        (option) => String(option.value) === String(selected)
      );

      return foundOption?.label || String(selected);
    },
    [options, placeholder]
  );

  return (
    <FormControl
      fullWidth={fullWidth}
      error={error}
      disabled={disabled}
      required={required}
      size={size}
      variant={variant}
      className={className}
    >
      {label && (
        <InputLabel id={labelId} required={required}>
          {label}
        </InputLabel>
      )}

      <Select
        {...rest}
        id={id}
        labelId={labelId}
        label={label}
        value={value}
        onChange={handleChange}
        renderValue={renderValue}
        displayEmpty={!!placeholder}
      >
        {placeholder && (
          <MenuItem value="" disabled>
            <em>{placeholder}</em>
          </MenuItem>
        )}

        {options.map((option) => (
          <MenuItem
            key={String(option.value)}
            value={String(option.value)}
            disabled={option.disabled}
          >
            {option.label}
          </MenuItem>
        ))}
      </Select>

      {helperText && <FormHelperText>{helperText}</FormHelperText>}
    </FormControl>
  );
}

// Memoized export of the custom select wrapper
export const CustomSelect = React.memo(CustomSelectComponent) as <
  T = string | number
>(
  props: CustomSelectProps<T> & { ref?: React.Ref<HTMLDivElement> }
) => React.ReactElement;

// RHF wrapper props
export interface SelectFieldProps<
  TFieldValues extends FieldValues = FieldValues,
  TValue = string | number
> extends Omit<
    CustomSelectProps<TValue>,
    "value" | "onChange" | "error" | "helperText"
  > {
  name: FieldPath<TFieldValues>;
  control: Control<TFieldValues>;
  rules?: ControllerProps<TFieldValues, FieldPath<TFieldValues>>["rules"];
  shouldUnregister?: boolean;
}

// RHF wrapper over the custom select
export function SelectField<
  TFieldValues extends FieldValues = FieldValues,
  TValue = string | number
>({
  name,
  control,
  rules,
  shouldUnregister,
  ...selectProps
}: SelectFieldProps<TFieldValues, TValue>) {
  return (
    <Controller
      name={name}
      control={control}
      rules={rules}
      shouldUnregister={shouldUnregister}
      render={({ field, fieldState }) => (
        <CustomSelect<TValue>
          {...selectProps}
          value={field.value ?? ""}
          onChange={(value) => {
            field.onChange(value === "" ? undefined : value);
          }}
          error={!!fieldState.error}
          helperText={fieldState.error?.message}
        />
      )}
    />
  );
}
