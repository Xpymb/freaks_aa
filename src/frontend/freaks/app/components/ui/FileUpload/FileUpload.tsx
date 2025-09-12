// FileUploadWithPreview.tsx
"use client";

import { useCallback, useEffect, useState } from "react";
import { useDropzone, FileRejection } from "react-dropzone";
import CustomModal from "@/components/ui/CustomModal/CustomModal";
import IconButton from "@mui/material/IconButton";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import ArrowBackIosNewIcon from "@mui/icons-material/ArrowBackIosNew";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import { useDisclosure } from "../useDisclosure";
import { CustomTypography } from "../CustomTypography";
import styles from "./_styles.module.scss";
import CustomImage from "../CustomImage";
import clsx from "clsx";
import { useUploadRaidScreenshot } from "@/domains/raids/hooks/useUploadRaidScreenshot";

type FilePreview = { file: File; preview: string };

type Props = {
  raidId: number;
  fileType?: number;
};

export default function FileUploadWithPreview({ raidId, fileType = 1 }: Props) {
  const [draftFiles, setDraftFiles] = useState<FilePreview[]>([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const { open, onOpen, onClose } = useDisclosure();

  const { trigger, isMutating, progress, error, errorState } =
    useUploadRaidScreenshot();

  const prev = () =>
    setCurrentIndex((i) => (i === 0 ? draftFiles.length - 1 : i - 1));
  const next = () =>
    setCurrentIndex((i) => (i === draftFiles.length - 1 ? 0 : i + 1));

  useEffect(() => {
    if (!open) return;
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === "ArrowLeft") prev();
      if (e.key === "ArrowRight") next();
    };
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [open, draftFiles.length]);

  const addFiles = useCallback(
    (newFiles: File[]) => {
      const mapped = newFiles.map((file) => ({
        file,
        preview: URL.createObjectURL(file),
      }));
      setDraftFiles((prev) => {
        const next = [...prev, ...mapped];
        setCurrentIndex(next.length - 1);
        return next;
      });
      if (!open) onOpen();
    },
    [onOpen, open]
  );

  const onDrop = useCallback(
    (accepted: File[], _rejected: FileRejection[]) => {
      if (accepted.length) addFiles(accepted);
    },
    [addFiles]
  );

  const {
    getRootProps,
    getInputProps,
    isDragActive,
    open: openFileDialog,
  } = useDropzone({
    onDrop,
    multiple: true,
    accept: { "image/*": [] },
    maxSize: 5 * 1024 * 1024,
    noClick: true,
    useFsAccessApi: true,
  });

  // paste (Ctrl+V)
  useEffect(() => {
    const handlePaste = (e: ClipboardEvent) => {
      const files: File[] = [];
      for (const item of e.clipboardData?.items ?? []) {
        if (item.kind === "file") {
          const f = item.getAsFile();
          if (f) files.push(f);
        }
      }
      if (files.length) addFiles(files);
    };
    window.addEventListener("paste", handlePaste);
    return () => window.removeEventListener("paste", handlePaste);
  }, [addFiles]);

  // освобождаем blob-URL
  useEffect(() => {
    return () => {
      draftFiles.forEach((f) => URL.revokeObjectURL(f.preview));
    };
  }, [draftFiles]);

  const handleUpload = async () => {
    if (!draftFiles.length || isMutating) return;

    try {
      for (const f of draftFiles) {
        await trigger(
          { raidId, fileType, file: f.file, optimistic: true },
          { throwOnError: true }
        );
      }

      setDraftFiles([]);
      onClose();
    } catch {
      console.error(
        errorState?.message ?? "Ошибка загрузки",
        errorState?.message
      );
    }
  };

  const handleClose = () => {
    setDraftFiles([]);
    onClose();
  };

  return (
    <div className={styles.wrapper}>
      {/* зона на странице */}
      <div
        {...getRootProps()}
        onClick={openFileDialog}
        className={clsx(styles.uploadZone, {
          [styles.isDragActive]: isDragActive && !open,
        })}
      >
        <input {...getInputProps()} />
        <CustomTypography variant="body2">
          {isDragActive
            ? "Бросай скриншоты сюда…"
            : "Перетащи, вставь Ctrl+V или выбери файлы (до 5MB)"}
        </CustomTypography>
      </div>

      {/* модалка превью + добавление ещё файлов */}
      <CustomModal
        title="Загрузка файлов"
        className={styles.modal}
        open={open}
        onClose={handleClose}
      >
        {draftFiles.length > 0 && (
          <div className={styles.wrapper}>
            {draftFiles.length > 1 && (
              <>
                <IconButton
                  onClick={prev}
                  style={{ position: "absolute", left: 10, top: "50%" }}
                >
                  <ArrowBackIosNewIcon />
                </IconButton>
                <IconButton
                  onClick={next}
                  style={{ position: "absolute", right: 10, top: "50%" }}
                >
                  <ArrowForwardIosIcon />
                </IconButton>
              </>
            )}

            <div className={styles.imageWrap}>
              <CustomImage
                className={styles.image}
                src={draftFiles[currentIndex].preview}
                alt={draftFiles[currentIndex].file.name}
              />
            </div>

            {draftFiles.length > 1 && (
              <CustomTypography variant="caption">
                {currentIndex + 1} / {draftFiles.length}
              </CustomTypography>
            )}

            {/* в модалке тоже можно добавить ещё */}
            <div
              {...getRootProps()}
              onClick={openFileDialog}
              className={clsx(styles.uploadZone, {
                [styles.isDragActive]: isDragActive && open,
              })}
            >
              <input {...getInputProps()} />
              <Typography variant="caption" color="textSecondary">
                Добавить ещё скриншоты (Drag’n’Drop / Ctrl+V / выбор)
              </Typography>
            </div>

            {/* прогресс/кнопки */}
            <div className={styles.btnWrapp}>
              <Button
                variant="contained"
                onClick={handleUpload}
                disabled={isMutating}
              >
                {isMutating ? `Загружаем… ${progress}%` : "Загрузить"}
              </Button>
              <Button
                variant="outlined"
                onClick={handleClose}
                disabled={isMutating}
              >
                Отмена
              </Button>
            </div>
          </div>
        )}
      </CustomModal>
    </div>
  );
}
