"use client";

import { IconButton, OutlinedInput } from "@mui/material";
import { Add, Remove } from "@mui/icons-material";
import {
  type Control,
  Controller,
  type ControllerProps,
  type FieldPath,
  type FieldValues,
} from "react-hook-form";
import styles from "./_styles.module.scss";
import { CustomTypography } from "../CustomTypography";

type Props = {
  value: number;
  onChange: (value: number) => void;
  min?: number;
  max?: number;
  step?: number;
  disabled?: boolean;
  label?: string;
  size?: "small" | "medium";
};

const NumberInput = ({
  value,
  onChange,
  min = 1,
  max = 999,
  step = 1,
  disabled = false,
  label,
  size = "small",
}: Props) => {
  const handleIncrement = () => {
    const newValue = Math.min(value + step, max);
    onChange(newValue);
  };

  const handleDecrement = () => {
    const newValue = Math.max(value - step, min);
    onChange(newValue);
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = parseInt(e.target.value, 10);
    if (!isNaN(newValue) && newValue >= min && newValue <= max) {
      onChange(newValue);
    }
  };

  return (
    <div className={styles.numberInputContainer}>
      {label && (
        <CustomTypography variant="caption" className={styles.label}>
          {label}
        </CustomTypography>
      )}
      <div className={styles.numberInput}>
        <IconButton
          onClick={handleDecrement}
          disabled={disabled || value <= min}
          size={size}
          className={styles.button}
        >
          <Remove />
        </IconButton>

        <OutlinedInput
          value={value}
          onChange={handleInputChange}
          disabled={disabled}
          size={size}
          inputProps={{
            min,
            max,
            type: "number",
            style: { textAlign: "center" },
          }}
          className={styles.input}
        />

        <IconButton
          onClick={handleIncrement}
          disabled={disabled || value >= max}
          size={size}
          className={styles.button}
        >
          <Add />
        </IconButton>
      </div>
    </div>
  );
};

export default NumberInput;

/* ================= RHF wrapper ================= */

type FieldNumberInputProps<FV extends FieldValues> = Omit<
  Props,
  "value" | "onChange"
> & {
  control: Control<FV>;
  name: FieldPath<FV>;
  rules?: ControllerProps<FV>["rules"];
};

export function NumberInputField<FV extends FieldValues>({
  control,
  name,
  rules,
  ...ui
}: FieldNumberInputProps<FV>) {
  return (
    <Controller
      control={control}
      name={name}
      rules={rules}
      render={({ field, fieldState }) => (
        <div>
          <NumberInput
            {...ui}
            value={(field.value ?? 1) as number}
            onChange={field.onChange}
          />
          {fieldState.error && (
            <div className={styles.errorText}>{fieldState.error.message}</div>
          )}
        </div>
      )}
    />
  );
}
