"use client";

import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";
import { IRaidScreenshot, RaidItem } from "@/domains/raids";
import { Avatar, AvatarGroup, IconButton } from "@mui/material";
import CustomImage from "@/components/ui/CustomImage";
import ImageViewerModal from "@/components/ui/ImageViewerModal/ImageViewerModal";
import { useDisclosure } from "@/components/ui/useDisclosure";
import { useState } from "react";
import { useAppError } from "@/shared/errors";
import ZoomOutMapIcon from "@mui/icons-material/ZoomOutMap";

type Props = {
  raid: RaidItem;
  screenshotData: {
    screenshots: IRaidScreenshot[];
    isLoading: boolean;
    errorState: ReturnType<typeof useAppError>;
  };
};

const Overview = ({ raid, screenshotData }: Props) => {
  const [screenshot, setScreenshot] = useState<string | null>(null);

  const { open, onOpen, onClose } = useDisclosure();

  return (
    <div className={styles.overview}>
      <div className={styles.wrapper}>
        <div className={styles.raidDescription}>
          <CustomTypography variant="h2">Описание</CustomTypography>
          <div className={styles.description}>
            <CustomTypography variant="subtitle1">
              {raid.description}
            </CustomTypography>
          </div>
        </div>
        <div className={styles.screenshots}>
          <CustomTypography variant="h2">Скриншоты</CustomTypography>
          <div className={styles.screenshotsList}>
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
      <aside className={styles.aside}>
        <div className={styles.participants}>
          <CustomTypography variant="h3">Участники</CustomTypography>
          <div className={styles.participantsList}>
            <AvatarGroup max={4}>
              <Avatar alt="Remy Sharp" />
              <Avatar alt="Travis Howard" />
              <Avatar alt="Cindy Baker" />
              <Avatar alt="Agnes Walker" />
              <Avatar alt="Trevor Henderson" />
            </AvatarGroup>
          </div>
        </div>
        <div className={styles.loot}>
          <CustomTypography variant="h3">Лут</CustomTypography>
          <div className={styles.lootList}>
            <div className={styles.lootItem}>
              <div className={styles.title}>
                <span
                  style={{
                    width: "15px",
                    height: "15px",
                    backgroundColor: "red",
                    borderRadius: "10px",
                  }}
                />
                <CustomTypography variant="caption">
                  Эссенция ярости
                </CustomTypography>
              </div>
              <div className={styles.quantity}>13</div>
            </div>
          </div>
        </div>
      </aside>
    </div>
  );
};

export default Overview;
