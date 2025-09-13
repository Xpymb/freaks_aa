"use client";

import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";
import {
  IRaidScreenshot,
  RaidItem,
  RaidLootDto,
  RaidParticipantDto,
  useGetRaidLoot,
} from "@/domains/raids";
import { Avatar, AvatarGroup } from "@mui/material";
import Screenshots from "../Screenshots/Screenshots";
import { useGetRaidParticipants } from "@/domains/raids/hooks/useGetRaidParticipants";
import { stringAvatar } from "@/layouts/Header/components/Profile/Profile";
import CustomImage from "@/components/ui/CustomImage";
import { useGetRaidScreenshots } from "@/domains/raids/hooks/useGetScreenshot";

type Props = {
  raid: RaidItem;
  prefetchParticipants: RaidParticipantDto[];
  prefetchLoot: RaidLootDto[];
  prefetchScreenshots: IRaidScreenshot[];
};

const Overview = ({
  raid,
  prefetchScreenshots,
  prefetchParticipants,
  prefetchLoot,
}: Props) => {
  const {
    data: participants,
    isLoading: participantsLoading,
    errorState: participantsErrorState,
  } = useGetRaidParticipants(prefetchParticipants, raid.id);

  const {
    lootItems,
    isLoading: lootLoading,
    errorState: lootErrorState,
  } = useGetRaidLoot(prefetchLoot, raid.id);

  const {
    screenshots,
    isLoading: screenshotsLoading,
    errorState: screenshotsErrorState,
  } = useGetRaidScreenshots(prefetchScreenshots, raid.id);

  return (
    <div className={styles.overview}>
      <div className={styles.wrapper}>
        <div className={styles.raidDescription}>
          <CustomTypography variant="h2">Описание</CustomTypography>
          <div className={styles.description}>
            {!raid.description && (
              <CustomTypography variant="subtitle1">
                Описание отсутствует.
              </CustomTypography>
            )}
            <CustomTypography variant="subtitle1">
              {raid.description}
            </CustomTypography>
          </div>
        </div>
        <div className={styles.screenshots}>
          <CustomTypography variant="h2">Скриншоты</CustomTypography>
          <Screenshots
            raid={raid}
            screenshotData={{
              screenshots,
              isLoading: screenshotsLoading,
              errorState: screenshotsErrorState,
            }}
            overview={true}
            maxOnPage={3}
          />
        </div>
      </div>
      <aside className={styles.aside}>
        <div className={styles.participants}>
          <CustomTypography variant="h3">Участники</CustomTypography>
          <div className={styles.participantsList}>
            <AvatarGroup max={4}>
              {participants?.map((p) => (
                <Avatar
                  key={p.participant.id}
                  alt={p.participant.gameNickname}
                  {...stringAvatar(p.participant.gameNickname)}
                />
              ))}
            </AvatarGroup>
          </div>
        </div>
        <div className={styles.loot}>
          <CustomTypography variant="h3">Лут</CustomTypography>
          <div className={styles.lootList}>
            {lootItems.map((item) => (
              <div key={item.loot.id} className={styles.lootItem}>
                <div className={styles.title}>
                  <div className={styles.iconWrapper}>
                    {item.loot.iconUri ? (
                      <div className={styles.iconContainer}>
                        <CustomImage
                          src={`${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${item.loot.iconUri}`}
                          alt={item.loot.description}
                          fill
                          className={styles.icon}
                        />
                        <CustomImage
                          src={`/images/masks/icon_grade${
                            item.loot.gradeType - 1
                          }.png`}
                          alt="Grade mask"
                          fill
                          className={styles.gradeMask}
                        />
                      </div>
                    ) : (
                      <div className={styles.colorIcon} />
                    )}
                  </div>

                  <CustomTypography variant="subtitle1">
                    {item.loot.name}
                  </CustomTypography>
                </div>
                <div className={styles.quantity}>{item.quantity}</div>
              </div>
            ))}
          </div>
        </div>
      </aside>
    </div>
  );
};

export default Overview;
