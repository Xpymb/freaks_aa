"use client";

import React, { useState, useEffect, useCallback } from "react";
import { Backdrop } from "@mui/material";
import Image from "next/image";
import styles from "./_styles.module.scss";

interface ImageViewerModalProps {
  open: boolean;
  onClose: () => void;
  imageUrl: string;
  alt?: string;
}

const ImageViewerModal: React.FC<ImageViewerModalProps> = ({
  open,
  onClose,
  imageUrl,
  alt = "Image",
}) => {
  const [imageDimensions, setImageDimensions] = useState<{
    width: number;
    height: number;
  } | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (!open || !imageUrl) return;

    setIsLoading(true);
    const img = new window.Image();

    img.onload = () => {
      const fixedWidth = Math.min(window.innerWidth * 0.8, 1200);
      const fixedHeight = Math.min(window.innerHeight * 0.8, 800);

      setImageDimensions({
        width: Math.round(fixedWidth),
        height: Math.round(fixedHeight),
      });
      setIsLoading(false);
    };

    img.onerror = () => {
      setIsLoading(false);
    };

    img.src = imageUrl;
  }, [open, imageUrl]);

  const handleBackdropClick = (event: React.MouseEvent) => {
    const target = event.target as HTMLElement;
    if (!target.closest("[data-close-button]")) {
      onClose();
    }
  };

  const handleKeyDown = useCallback(
    (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        onClose();
      }
    },
    [onClose]
  );

  useEffect(() => {
    if (open) {
      document.addEventListener("keydown", handleKeyDown);
      document.body.style.overflow = "hidden";
    }

    return () => {
      document.removeEventListener("keydown", handleKeyDown);
      document.body.style.overflow = "unset";
    };
  }, [open, handleKeyDown]);

  if (!open) return null;

  return (
    <Backdrop
      open={open}
      className={styles.backdrop}
      onClick={handleBackdropClick}
    >
      <div className={styles.container}>
        {isLoading ? (
          <div className={styles.loader}>
            <div className={styles.spinner} />
          </div>
        ) : imageDimensions ? (
          <>
            <div
              className={styles.imageContainer}
              style={{
                width: imageDimensions.width,
                height: imageDimensions.height,
              }}
            >
              <Image
                src={imageUrl}
                alt={alt}
                width={imageDimensions.width}
                height={imageDimensions.height}
                className={styles.image}
                priority
              />
            </div>
          </>
        ) : (
          <div className={styles.errorMessage}>
            Не удалось загрузить изображение
          </div>
        )}
      </div>
    </Backdrop>
  );
};

export default ImageViewerModal;
