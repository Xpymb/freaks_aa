import { Breakpoint, Container, ContainerProps } from "@mui/material";

interface CustomContainerProps extends ContainerProps {
  maxWidth: Breakpoint;
}

const CustomContainer = ({
  children,
  maxWidth,
  ...props
}: CustomContainerProps) => {
  return (
    <Container maxWidth={maxWidth} {...props}>
      {children}
    </Container>
  );
};

export { CustomContainer };
