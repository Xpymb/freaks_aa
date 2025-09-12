import { IRaidScreenshot, RaidItem } from "@/domains/raids";
import styles from "./_styles.module.scss";
import CustomImage from "@/components/ui/CustomImage";
import { useState } from "react";
import { useDisclosure } from "@/components/ui/useDisclosure";
import FileUploadWithPreview from "@/components/ui/FileUpload/FileUpload";
import { useGetRaidScreenshots } from "@/domains/raids/hooks/useGetScreenshot";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";
import NotFound from "@/components/ui/NotFound/NotFound";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import DeleteForeverIcon from "@mui/icons-material/DeleteForever";
import { IconButton } from "@mui/material";
import ZoomOutMapIcon from "@mui/icons-material/ZoomOutMap";
import { useDeleteRaidScreenshot } from "@/domains/raids/hooks/useDeleteRaidScreenshot";
import { useAppError } from "@/shared/errors";
import ImageViewerModal from "@/components/ui/ImageViewerModal/ImageViewerModal";

type Props = {
  raid: RaidItem;
  screenshotData: {
    screenshots: IRaidScreenshot[];
    isLoading: boolean;
    errorState: ReturnType<typeof useAppError>;
  };
};

const Screenshots = ({ raid, screenshotData }: Props) => {
  const [screenshot, setScreenshot] = useState<string | null>(
    null
  );
  const { open, onOpen, onClose } = useDisclosure();

  const {
    trigger: deleteShot,
    isMutating,
    errorState,
  } = useDeleteRaidScreenshot();

  const handleDelete =
    (url: string) => async (e: React.MouseEvent<HTMLButtonElement>) => {
      e.stopPropagation();
      try {
        await deleteShot({ raidId: raid.id, url }, { throwOnError: true });
      } catch {

      }
    };

  if (screenshotData.isLoading || !screenshotData.screenshots)
    return <DefaultLoader />;
  if (screenshotData.errorState.isError)
    return <ErrorLoadData message={screenshotData.errorState.message} />;

  return (
    <div className={styles.screenshots}>
      <FileUploadWithPreview raidId={raid.id} />
      <div className={styles.wrapper}>
        <div className={styles.screenshotsList}>
          {screenshotData.screenshots.length === 0 && <NotFound message="" />}
          {screenshotData.screenshots.map((s, i) => (
            <div
              key={i}
              onClick={() => {
                onOpen();
                setScreenshot(
                  `${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${s.screenshotUri}`
                );
              }}
              className={styles.imageWrap}
            >
              <IconButton
                className={styles.deleteBtn}
                onClick={handleDelete(s.screenshotUri)}
                disabled={isMutating}
                aria-label="Удалить скриншот"
              >
                <DeleteForeverIcon />
              </IconButton>

              <IconButton className={styles.zoomBtn}>
                <ZoomOutMapIcon />
              </IconButton>

              <CustomImage
                className={styles.image}
                alt="raid screenshot"
                src={`${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${s.screenshotUri}`}
                fill
              />
            </div>
          ))}
          <ImageViewerModal
              open={open}
              onClose={onClose}
              imageUrl={screenshot || ""}
              alt="Скриншот рейда"
            />
        </div>
      </div>
    </div>
  );
};

export default Screenshots;
