import { IconButton, Tooltip } from "@mui/material";
import HelpOutlineIcon from "@mui/icons-material/HelpOutline";

type HelpHintProps = {
  title: React.ReactNode;
  placement?: "top" | "bottom" | "left" | "right";
  size?: "small" | "medium";
};

export function HelpHint({
  title,
  placement = "top",
  size = "small",
}: HelpHintProps) {
  return (
    <Tooltip title={title} placement={placement} arrow>
      <IconButton
        size={size}
        aria-label="Помощь"
        color="inherit"
        disableRipple
        sx={{
          ml: 0.5,
          p: 0.25,
          opacity: 0.7,
          "&:hover": { opacity: 1 },
        }}
      >
        <HelpOutlineIcon fontSize="small" />
      </IconButton>
    </Tooltip>
  );
}
