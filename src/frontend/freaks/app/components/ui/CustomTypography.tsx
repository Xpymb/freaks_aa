import { Typography, TypographyProps, TypographyTypeMap } from "@mui/material";

interface CustomTypographyProps extends TypographyProps {
  variant: TypographyTypeMap["props"]["variant"];
}

const CustomTypography = ({
  variant,
  fontWeight,
  children,
  ...props
}: CustomTypographyProps) => {
  return (
    <Typography
      variant={variant}
      sx={{ zIndex: 2 }}
      fontWeight={fontWeight}
      {...props}
    >
      {children}
    </Typography>
  );
};

export { CustomTypography };
