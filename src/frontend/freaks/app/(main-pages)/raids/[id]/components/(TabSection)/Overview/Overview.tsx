"use client";

import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";
import {
  IRaidScreenshot,
  RaidItem,
  RaidLootDto,
  RaidParticipantDto,
} from "@/domains/raids";
import { Avatar, AvatarGroup } from "@mui/material";
import Screenshots from "../Screenshots/Screenshots";
import { stringAvatar } from "@/layouts/Header/components/Profile/Profile";
import CustomImage from "@/components/ui/CustomImage";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import NotFound from "@/components/ui/NotFound/NotFound";
import { useAppError } from "@/shared/errors";

type Props = {
  raid: RaidItem;
  participants: RaidParticipantDto[];
  participantsLoading: boolean;
  participantsError: { isError: boolean; message?: string } | null;
  loot: RaidLootDto[];
  lootLoading: boolean;
  lootError: { isError: boolean; message?: string } | null;
  screenshots: IRaidScreenshot[];
  screenshotsLoading: boolean;
  screenshotsError: ReturnType<typeof useAppError>;
};

const Overview = ({
  raid,
  participants,
  participantsLoading,
  participantsError,
  loot,
  lootLoading,
  lootError,
  screenshots,
  screenshotsLoading,
  screenshotsError,
}: Props) => {
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
              errorState: screenshotsError,
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
            <AvatarGroup max={4} className={styles.avatarGroup}>
              {participantsLoading && <DefaultLoader />}
              {participantsError?.isError && (
                <ErrorLoadData message={participantsError?.message} />
              )}
              {participants?.length === 0 && (
                <NotFound
                  message="Не указано"
                  variant="subtitle1"
                  align="start"
                />
              )}
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
            {lootLoading && <DefaultLoader />}
            {lootError?.isError && (
              <ErrorLoadData message={lootError?.message} />
            )}
            {loot.length === 0 && (
              <NotFound
                message="Не указано"
                variant="subtitle1"
                align="start"
              />
            )}
            {loot.map((item) => (
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
