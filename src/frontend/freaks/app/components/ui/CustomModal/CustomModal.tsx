import * as React from "react";
import { Modal, IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import clsx from "clsx";
import styles from "./_styles.module.scss";

export type MinimalModalProps = {
  open: boolean;
  onClose: () => void;

  title?: React.ReactNode;
  children: React.ReactNode;
  footer?: React.ReactNode;

  showClose?: boolean;

  className?: string;
  headerClassName?: string;
  bodyClassName?: string;
  footerClassName?: string;

  minWidth?: string | number;
};

const getTitleText = (t: React.ReactNode): string | undefined => {
  if (typeof t === "string" || typeof t === "number") return String(t);

  if (React.isValidElement<{ children?: React.ReactNode }>(t)) {
    const c = t.props.children;
    if (typeof c === "string" || typeof c === "number") return String(c);
  }

  return undefined;
};

const CustomModal = ({
  open,
  onClose,
  title,
  children,
  footer,
  showClose = true,
  className,
  headerClassName,
  bodyClassName,
  footerClassName,
  minWidth = "min(700px, 92vw)",
}: MinimalModalProps) => {
  const titleAttr = getTitleText(title);

  return (
    <Modal open={open} onClose={onClose}>
      <div style={{ minWidth }} className={clsx(styles.card, className)}>
        {(title != null || showClose) && (
          <div className={clsx(styles.header, headerClassName)}>
            {title && (
              <div className={styles.title} title={titleAttr}>
                {title}
              </div>
            )}
            {showClose && (
              <IconButton size="small" aria-label="close" onClick={onClose}>
                <CloseIcon />
              </IconButton>
            )}
          </div>
        )}

        <div className={clsx(styles.body, bodyClassName)}>{children}</div>

        {footer && (
          <div className={clsx(styles.footer, footerClassName)}>{footer}</div>
        )}
      </div>
    </Modal>
  );
};

export default CustomModal;
