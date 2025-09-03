import TextField, { TextFieldProps } from "@mui/material/TextField";
import {
  FieldError,
  FieldValues,
  Path,
  UseFormRegister,
} from "react-hook-form";
import { CustomTypography } from "../CustomTypography";

type CustomInputProps<T extends FieldValues> = {
  name: Path<T>;
  label: string;
  register: UseFormRegister<T>;
  error?: FieldError | undefined;
  helperText?: string;
} & Omit<TextFieldProps, "error">;

const CustomInput = <T extends FieldValues>({
  name,
  label,
  register,
  error,
  helperText,
  ...props
}: CustomInputProps<T>) => {
  const customErrorText = (
    <CustomTypography color="error" variant="overline">
      {error?.message}
    </CustomTypography>
  );
  const customHelpText = (
    <CustomTypography variant="overline">{helperText}</CustomTypography>
  );
  return (
    <TextField
      label={label}
      {...register(name)}
      error={Boolean(error)}
      helperText={error ? customErrorText : customHelpText}
      {...props}
    />
  );
};

export default CustomInput;
