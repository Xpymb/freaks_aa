import {
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  FormHelperText,
  SelectProps,
} from "@mui/material";
import {
  ControllerRenderProps,
  FieldError,
  FieldValues,
  Path,
  UseFormRegister,
} from "react-hook-form";
import { CustomTypography } from "../CustomTypography";
import { ReactNode } from "react";

type CustomSelectProps<T extends FieldValues> = {
  name: Path<T>;
  label: string;
  options: { value: string | number; label: ReactNode; disabled?: boolean }[];
  field: ControllerRenderProps<T, Path<T>>;
  error?: FieldError;
  helperText?: string;
  defaultValue?: string | number;
} & Omit<SelectProps, "error">;

const CustomSelect = <T extends FieldValues>({
  name,
  label,
  options,
  field,
  error,
  helperText,
  defaultValue = "",
  ...props
}: CustomSelectProps<T>) => {
  const customErrorText = (
    <CustomTypography color="error" variant="overline">
      {error?.message}
    </CustomTypography>
  );
  const customHelpText = (
    <CustomTypography variant="overline">{helperText}</CustomTypography>
  );

  return (
    <FormControl fullWidth margin="normal" error={Boolean(error)}>
      <InputLabel id={`${name}-label`}>{label}</InputLabel>
      <Select
        labelId={`${name}-label`}
        label={label}
        defaultValue={defaultValue}
        {...field}
        {...props}
      >
        {options.map((option) => (
          <MenuItem
            disabled={option.disabled}
            key={option.value}
            value={option.value}
          >
            {option.label}
          </MenuItem>
        ))}
      </Select>
      <FormHelperText>
        {error ? customErrorText : customHelpText}
      </FormHelperText>
    </FormControl>
  );
};

export default CustomSelect;
