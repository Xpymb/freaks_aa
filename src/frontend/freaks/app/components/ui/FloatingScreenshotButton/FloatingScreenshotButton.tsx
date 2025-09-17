"use client";

import React, { useState, useRef, useCallback } from "react";
import { Portal } from "@mui/material";
import {
  IconButton,
  Slider,
  Typography,
  Tooltip,
  Chip,
  Fade,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
} from "@mui/material";
import {
  PhotoCamera as CameraIcon,
  Visibility as VisibilityIcon,
  VisibilityOff as VisibilityOffIcon,
  Fullscreen as FullscreenIcon,
  Close as CloseIcon,
  NavigateBefore as PrevIcon,
  NavigateNext as NextIcon,
  CenterFocusStrong as CenterIcon,
  RestartAlt as ResetIcon,
} from "@mui/icons-material";
import CustomImage from "@/components/ui/CustomImage";
import ImageViewerModal from "@/components/ui/ImageViewerModal/ImageViewerModal";
import { useDisclosure } from "@/components/ui/useDisclosure";
import styles from "./_styles.module.scss";

interface Screenshot {
  id: string;
  url: string;
  thumbnail?: string;
}

interface FloatingScreenshotButtonProps {
  screenshots: Screenshot[];
  isVisible?: boolean;
}

const FloatingScreenshotButton = ({
  screenshots,
  isVisible = true,
}: FloatingScreenshotButtonProps) => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [selectedScreenshot, setSelectedScreenshot] =
    useState<Screenshot | null>(null);
  const [currentScreenshotIndex, setCurrentScreenshotIndex] = useState(0);
  const [opacity, setOpacity] = useState(0.7);
  const [isOverlayVisible, setIsOverlayVisible] = useState(false);

  const [overlayPosition, setOverlayPosition] = useState({ x: 0, y: 0 });
  const [overlaySize, setOverlaySize] = useState({ width: 400, height: 300 });
  const [isDragging, setIsDragging] = useState(false);
  const [isResizing, setIsResizing] = useState(false);
  const [dragStart, setDragStart] = useState({ x: 0, y: 0 });
  const [resizeStart, setResizeStart] = useState({
    x: 0,
    y: 0,
    width: 0,
    height: 0,
  });

  const overlayRef = useRef<HTMLDivElement>(null);
  const {
    open: fullscreenOpen,
    onOpen: onFullscreenOpen,
    onClose: onFullscreenClose,
  } = useDisclosure();

  const handleButtonClick = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const handleScreenshotSelect = (screenshot: Screenshot, index: number) => {
    setSelectedScreenshot(screenshot);
    setCurrentScreenshotIndex(index);
    setIsOverlayVisible(true);
    setIsMenuOpen(false);

    if (overlayPosition.x === 0 && overlayPosition.y === 0) {
      const centerX = (window.innerWidth - overlaySize.width) / 2;
      const centerY = (window.innerHeight - overlaySize.height) / 2;
      setOverlayPosition({ x: centerX, y: centerY });
    }
  };

  const handlePrevScreenshot = () => {
    const prevIndex =
      currentScreenshotIndex > 0
        ? currentScreenshotIndex - 1
        : screenshots.length - 1;
    setCurrentScreenshotIndex(prevIndex);
    setSelectedScreenshot(screenshots[prevIndex]);
  };

  const handleNextScreenshot = () => {
    const nextIndex =
      currentScreenshotIndex < screenshots.length - 1
        ? currentScreenshotIndex + 1
        : 0;
    setCurrentScreenshotIndex(nextIndex);
    setSelectedScreenshot(screenshots[nextIndex]);
  };

  const handleOverlayClose = () => {
    setIsOverlayVisible(false);
    setSelectedScreenshot(null);
  };

  const handleFullscreen = () => {
    if (selectedScreenshot) {
      onFullscreenOpen();
    }
  };

  const handleOpacityChange = (event: Event, newValue: number | number[]) => {
    setOpacity(newValue as number);
  };

  const toggleOverlayVisibility = () => {
    setIsOverlayVisible(!isOverlayVisible);
  };

  const handleDragStart = useCallback(
    (e: React.MouseEvent) => {
      e.preventDefault();
      setIsDragging(true);
      setDragStart({
        x: e.clientX - overlayPosition.x,
        y: e.clientY - overlayPosition.y,
      });
    },
    [overlayPosition]
  );

  const handleDragMove = useCallback(
    (e: MouseEvent) => {
      if (!isDragging) return;

      const newX = e.clientX - dragStart.x;
      const newY = e.clientY - dragStart.y;

      const maxX = window.innerWidth - overlaySize.width;
      const maxY = window.innerHeight - overlaySize.height;

      setOverlayPosition({
        x: Math.max(0, Math.min(newX, maxX)),
        y: Math.max(0, Math.min(newY, maxY)),
      });
    },
    [isDragging, dragStart, overlaySize]
  );

  const handleDragEnd = useCallback(() => {
    setIsDragging(false);
  }, []);

  const handleResizeStart = useCallback(
    (e: React.MouseEvent) => {
      e.preventDefault();
      e.stopPropagation();
      setIsResizing(true);
      setResizeStart({
        x: e.clientX,
        y: e.clientY,
        width: overlaySize.width,
        height: overlaySize.height,
      });
    },
    [overlaySize]
  );

  const handleResizeMove = useCallback(
    (e: MouseEvent) => {
      if (!isResizing) return;

      const deltaX = e.clientX - resizeStart.x;
      const deltaY = e.clientY - resizeStart.y;

      const newWidth = Math.max(200, Math.min(800, resizeStart.width + deltaX));
      const newHeight = Math.max(
        150,
        Math.min(600, resizeStart.height + deltaY)
      );

      setOverlaySize({ width: newWidth, height: newHeight });
    },
    [isResizing, resizeStart]
  );

  const handleResizeEnd = useCallback(() => {
    setIsResizing(false);
  }, []);

  // Функция сброса позиции и размера
  const resetOverlayPosition = useCallback(() => {
    const centerX = (window.innerWidth - overlaySize.width) / 2;
    const centerY = (window.innerHeight - overlaySize.height) / 2;
    setOverlayPosition({ x: centerX, y: centerY });
  }, [overlaySize]);

  const resetOverlaySize = useCallback(() => {
    setOverlaySize({ width: 400, height: 300 });
  }, []);

  // Добавляем и удаляем глобальные обработчики событий
  React.useEffect(() => {
    if (isDragging) {
      document.addEventListener("mousemove", handleDragMove);
      document.addEventListener("mouseup", handleDragEnd);
      return () => {
        document.removeEventListener("mousemove", handleDragMove);
        document.removeEventListener("mouseup", handleDragEnd);
      };
    }
  }, [isDragging, handleDragMove, handleDragEnd]);

  React.useEffect(() => {
    if (isResizing) {
      document.addEventListener("mousemove", handleResizeMove);
      document.addEventListener("mouseup", handleResizeEnd);
      return () => {
        document.removeEventListener("mousemove", handleResizeMove);
        document.removeEventListener("mouseup", handleResizeEnd);
      };
    }
  }, [isResizing, handleResizeMove, handleResizeEnd]);

  if (!isVisible || screenshots.length === 0) return null;

  return (
    <>
      <Portal>
        <div className={styles.floatingButton}>
          <Tooltip title="Скриншоты рейда">
            <IconButton
              onClick={handleButtonClick}
              className={styles.button}
              size="large"
            >
              <CameraIcon />
              <Chip
                label={screenshots.length}
                size="small"
                className={styles.badge}
              />
            </IconButton>
          </Tooltip>

          <Fade in={isMenuOpen} timeout={200}>
            <div className={styles.menu}>
              <List className={styles.screenshotList}>
                {screenshots.map((screenshot, index) => (
                  <ListItem key={screenshot.id || index} disablePadding>
                    <ListItemButton
                      onClick={() => handleScreenshotSelect(screenshot, index)}
                      className={styles.screenshotItem}
                    >
                      <ListItemIcon className={styles.thumbnailContainer}>
                        <CustomImage
                          src={screenshot.thumbnail || screenshot.url}
                          alt={`Скриншот ${index + 1}`}
                          fill
                          className={styles.thumbnail}
                        />
                      </ListItemIcon>
                    </ListItemButton>
                  </ListItem>
                ))}
              </List>
            </div>
          </Fade>
        </div>

        {selectedScreenshot && (
          <div className={styles.overlayControls}>
            <div className={styles.controlPanel}>
              <div className={styles.controlGroup}>
                <Tooltip title="Предыдущий скриншот">
                  <IconButton
                    onClick={handlePrevScreenshot}
                    size="small"
                    className={styles.controlButton}
                  >
                    <PrevIcon />
                  </IconButton>
                </Tooltip>
                <Chip
                  label={`${currentScreenshotIndex + 1}/${screenshots.length}`}
                  size="small"
                  className={styles.counter}
                />
                <Tooltip title="Следующий скриншот">
                  <IconButton
                    onClick={handleNextScreenshot}
                    size="small"
                    className={styles.controlButton}
                  >
                    <NextIcon />
                  </IconButton>
                </Tooltip>
              </div>
              <div className={styles.opacityControl}>
                <Typography variant="caption" className={styles.sliderLabel}>
                  Прозрачность
                </Typography>
                <Slider
                  value={opacity}
                  onChange={handleOpacityChange}
                  min={0.1}
                  max={1}
                  step={0.1}
                  size="small"
                  className={styles.slider}
                  sx={{
                    color: "#2196f3",
                    "& .MuiSlider-thumb": {
                      backgroundColor: "#2196f3",
                    },
                    "& .MuiSlider-track": {
                      backgroundColor: "#2196f3",
                    },
                    "& .MuiSlider-rail": {
                      backgroundColor: "rgba(255, 255, 255, 0.3)",
                    },
                  }}
                />
              </div>
              <div className={styles.controlGroup}>
                <Tooltip
                  title={
                    isOverlayVisible ? "Скрыть оверлей" : "Показать оверлей"
                  }
                >
                  <IconButton
                    onClick={toggleOverlayVisibility}
                    size="small"
                    className={`${styles.controlButton} ${
                      isOverlayVisible ? styles.active : ""
                    }`}
                  >
                    {isOverlayVisible ? (
                      <VisibilityOffIcon />
                    ) : (
                      <VisibilityIcon />
                    )}
                  </IconButton>
                </Tooltip>
              </div>

              <Tooltip title="Полноэкранный режим">
                <IconButton
                  onClick={handleFullscreen}
                  size="small"
                  className={styles.controlButton}
                >
                  <FullscreenIcon />
                </IconButton>
              </Tooltip>

              <Tooltip title="Центрировать оверлей">
                <IconButton
                  onClick={resetOverlayPosition}
                  size="small"
                  className={styles.controlButton}
                  disabled={!isOverlayVisible}
                >
                  <CenterIcon />
                </IconButton>
              </Tooltip>

              <Tooltip title="Сбросить размер">
                <IconButton
                  onClick={resetOverlaySize}
                  size="small"
                  className={styles.controlButton}
                  disabled={!isOverlayVisible}
                >
                  <ResetIcon />
                </IconButton>
              </Tooltip>

              <Tooltip title="Закрыть">
                <IconButton
                  onClick={handleOverlayClose}
                  size="small"
                  className={styles.controlButton}
                >
                  <CloseIcon />
                </IconButton>
              </Tooltip>
            </div>
          </div>
        )}

        {isOverlayVisible && selectedScreenshot && (
          <div
            ref={overlayRef}
            className={`${styles.overlay} ${
              isDragging ? styles.dragging : ""
            } ${isResizing ? styles.resizing : ""}`}
            style={{
              opacity,
              left: overlayPosition.x,
              top: overlayPosition.y,
              width: overlaySize.width,
              height: overlaySize.height,
              cursor: isDragging ? "grabbing" : "grab",
            }}
            onMouseDown={handleDragStart}
          >
            <CustomImage
              src={selectedScreenshot.url}
              alt="Скриншот рейда"
              fill
              className={styles.overlayImage}
              sizes="100vw"
            />

            {/* Индикатор изменения размера */}
            <div
              className={styles.resizeHandle}
              onMouseDown={handleResizeStart}
            />
          </div>
        )}
      </Portal>

      {/* Полноэкранный просмотр */}
      <Portal>
        <ImageViewerModal
          open={fullscreenOpen}
          onClose={onFullscreenClose}
          imageUrl={selectedScreenshot?.url || ""}
          alt="Скриншот рейда"
        />
      </Portal>
    </>
  );
};

export default FloatingScreenshotButton;
