"use client";

import React, { useState, useEffect, useMemo } from "react";
import { Autocomplete, TextField, IconButton } from "@mui/material";
import { Close as CloseIcon } from "@mui/icons-material";
import {
  RaidParticipantDto,
  RaidItem,
  useRaidPermissions,
} from "@/domains/raids";
import { useRaidParticipantMutations } from "@/domains/raids/hooks/useRaidParticipantMutations";
import { useGetUsers } from "@/domains/user/hooks/useGetUsers";
import { formatProfileName } from "@/utils/formatProfileName";
import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";
import { IUser } from "@/domains/user/types";

type Props = {
  raidId: number;
  raid: RaidItem;
  participants: RaidParticipantDto[];
  prefetchUsers: IUser[];
  onParticipantsChange: () => void;
};

const PARTIES_PER_RAID = 10; // 10 отрядов на рейд
const POSITIONS_PER_PARTY = 5; // 5 позиций в отряде
const TOTAL_POSITIONS = PARTIES_PER_RAID * POSITIONS_PER_PARTY;

const ParticipantGrid = ({
  raidId,
  raid,
  participants,
  prefetchUsers,
  onParticipantsChange,
}: Props) => {
  // Проверяем права на редактирование участников
  const { canEdit } = useRaidPermissions(raid);
  const { createParticipant, deleteParticipant } =
    useRaidParticipantMutations(raidId);
  const { data: users = prefetchUsers, isLoading: usersLoading } = useGetUsers({
    fallbackData: prefetchUsers,
    includeWoRoles: false,
  });

  // Локальное состояние для дополнительных "заготовок" рейдов
  const [additionalRaids, setAdditionalRaids] = useState<number[]>([]);
  // Отслеживаем все рейды, которые когда-либо были созданы
  const [allCreatedRaids, setAllCreatedRaids] = useState<Set<number>>(() => {
    const existingRaids = new Set(participants.map((p) => p.raidNumber));
    existingRaids.add(1); // Первый рейд всегда существует
    return existingRaids;
  });

  // Группируем участников по номеру рейда
  const raidGroups = useMemo(() => {
    const groups: Record<number, RaidParticipantDto[]> = {};

    // Первый рейд всегда существует
    groups[1] = [];

    participants.forEach((participant) => {
      const raidNumber = participant.raidNumber;
      if (!groups[raidNumber]) {
        groups[raidNumber] = [];
      }
      groups[raidNumber].push(participant);
    });

    // Добавляем дополнительные рейды
    additionalRaids.forEach((raidNumber) => {
      if (!groups[raidNumber]) {
        groups[raidNumber] = [];
      }
    });

    return Object.keys(groups)
      .map((raidNumber) => ({
        raidNumber: parseInt(raidNumber),
        participants: groups[parseInt(raidNumber)],
      }))
      .sort((a, b) => a.raidNumber - b.raidNumber);
  }, [participants, additionalRaids]);

  // Создаем сетку участников для каждого рейда
  const createGridForRaid = (raidParticipants: RaidParticipantDto[]) => {
    const grid = new Array(TOTAL_POSITIONS).fill(null);

    raidParticipants.forEach((participant) => {
      const position =
        (participant.raidPartyNumber - 1) * POSITIONS_PER_PARTY +
        (participant.raidPartyPositionNumber - 1);
      if (position >= 0 && position < TOTAL_POSITIONS) {
        grid[position] = participant;
      }
    });

    return grid;
  };

  // Обработчик для добавления участника в определенный рейд
  const handleAddParticipant = async (
    user: IUser,
    position: number,
    raidNumber: number
  ) => {
    const partyNumber = Math.floor(position / POSITIONS_PER_PARTY) + 1;
    const positionInParty = (position % POSITIONS_PER_PARTY) + 1;

    // Проверяем, не участвует ли уже этот пользователь в любом рейде
    const userInAnyRaid = participants.find(
      (p) => p.participant.id === user.id
    );

    if (userInAnyRaid) {
      alert(
        `Пользователь ${formatProfileName(user)} уже участвует в рейде №${
          userInAnyRaid.raidNumber
        }`
      );
      return;
    }

    try {
      await createParticipant.trigger({
        participantId: user.id,
        raidNumber: raidNumber,
        raidPartyNumber: partyNumber,
        raidPartyPositionNumber: positionInParty,
      });

      onParticipantsChange();
    } catch (error) {
      console.error("Failed to add participant:", error);
    }
  };

  // Обработчик для удаления участника
  const handleRemoveParticipant = async (participant: RaidParticipantDto) => {
    try {
      await deleteParticipant.trigger(participant.participant.id);
      onParticipantsChange();
    } catch (error) {
      console.error("Failed to remove participant:", error);
    }
  };

  // Добавить новый рейд
  const handleAddRaid = () => {
    const newRaidNumber = maxRaidNumber + 1;
    setAdditionalRaids((prev) => [...prev, newRaidNumber]);
    setAllCreatedRaids((prev) => new Set([...prev, newRaidNumber]));
  };

  // Удалить рейд (удаляет всех участников данного рейда)
  const handleRemoveRaid = async (raidNumber: number) => {
    // Первый рейд нельзя удалять
    if (raidNumber === 1) {
      alert("Первый рейд не может быть удален");
      return;
    }

    const raidGroup = raidGroups.find(
      (group) => group.raidNumber === raidNumber
    );
    if (!raidGroup) return;

    // Формируем сообщение в зависимости от наличия участников
    const message =
      raidGroup.participants.length === 0
        ? `Удалить пустой рейд №${raidNumber}?`
        : `Удалить рейд №${raidNumber} и всех его участников (${raidGroup.participants.length} чел.)?`;

    const confirmDelete = window.confirm(message);
    if (!confirmDelete) return;

    try {
      // Удаляем всех участников этого рейда (если есть)
      for (const participant of raidGroup.participants) {
        await deleteParticipant.trigger(participant.participant.id);
      }

      // Удаляем рейд из дополнительных и из отслеживания
      setAdditionalRaids((prev) => prev.filter((num) => num !== raidNumber));
      setAllCreatedRaids((prev) => {
        const newSet = new Set(prev);
        newSet.delete(raidNumber);
        return newSet;
      });
      onParticipantsChange();
    } catch (error) {
      console.error("Failed to remove raid:", error);
    }
  };

  // Фильтруем пользователей - исключаем всех, кто уже участвует в любом рейде
  const getAvailableUsers = () => {
    const addedUserIds = new Set(participants.map((p) => p.participant.id));
    return users.filter((user) => !addedUserIds.has(user.id));
  };

  // Следим за созданными рейдами и возвращаем пустые в additionalRaids
  useEffect(() => {
    const raidsWithParticipants = new Set(
      participants.map((p) => p.raidNumber)
    );
    const emptyCreatedRaids: number[] = [];

    // Находим все созданные рейды, которые сейчас пусты
    allCreatedRaids.forEach((raidNumber) => {
      if (
        raidNumber > 1 &&
        !raidsWithParticipants.has(raidNumber) &&
        !additionalRaids.includes(raidNumber)
      ) {
        emptyCreatedRaids.push(raidNumber);
      }
    });

    // Добавляем пустые рейды обратно в additionalRaids
    if (emptyCreatedRaids.length > 0) {
      setAdditionalRaids((prev) => [...prev, ...emptyCreatedRaids]);
    }
  }, [participants, allCreatedRaids, additionalRaids]);

  // Обработчик изменения участников
  useEffect(() => {
    const handleRefresh = () => {
      onParticipantsChange();
    };

    window.addEventListener("refresh-participants", handleRefresh);
    return () => {
      window.removeEventListener("refresh-participants", handleRefresh);
    };
  }, [onParticipantsChange]);

  // Получаем максимальный номер рейда для кнопки добавления нового рейда
  const maxRaidNumber = Math.max(
    ...raidGroups.map((group) => group.raidNumber),
    0
  );

  return (
    <div className={styles.participantGrid}>
      {raidGroups.map((raidGroup) => {
        const gridData = createGridForRaid(raidGroup.participants);
        const availableUsers = getAvailableUsers();

        return (
          <div key={raidGroup.raidNumber} className={styles.raidContainer}>
            <div className={styles.raidHeader}>
              <CustomTypography variant="h6" className={styles.raidTitle}>
                Рейд {raidGroup.raidNumber} ({raidGroup.participants.length}/50)
              </CustomTypography>
              {canEdit && raidGroup.raidNumber > 1 && (
                <IconButton
                  size="small"
                  onClick={() => handleRemoveRaid(raidGroup.raidNumber)}
                  className={styles.removeRaidButton}
                >
                  <CloseIcon fontSize="small" />
                </IconButton>
              )}
            </div>

            <div className={styles.gridContainer}>
              {Array.from({ length: PARTIES_PER_RAID }, (_, partyIndex) => (
                <div key={partyIndex} className={styles.party}>
                  <CustomTypography variant="h6" className={styles.partyLabel}>
                    Отряд {partyIndex + 1}
                  </CustomTypography>
                  <div className={styles.partyGrid}>
                    {Array.from(
                      { length: POSITIONS_PER_PARTY },
                      (_, positionIndex) => {
                        const globalPosition =
                          partyIndex * POSITIONS_PER_PARTY + positionIndex;
                        const participant = gridData[globalPosition];
                        const isEmpty = !participant;

                        return (
                          <div
                            key={positionIndex}
                            className={`${styles.gridCell} ${
                              isEmpty ? styles.emptyCell : styles.filledCell
                            }`}
                          >
                            {participant ? (
                              <div className={styles.participantInfo}>
                                <CustomTypography
                                  variant="subtitle1"
                                  className={styles.participantName}
                                >
                                  {formatProfileName(participant.participant)}
                                </CustomTypography>
                                {canEdit && (
                                  <IconButton
                                    size="small"
                                    className={styles.removeButton}
                                    onClick={(e) => {
                                      e.stopPropagation();
                                      handleRemoveParticipant(participant);
                                    }}
                                  >
                                    <CloseIcon fontSize="small" />
                                  </IconButton>
                                )}
                              </div>
                            ) : canEdit ? (
                              <div className={styles.cellAutocomplete}>
                                <Autocomplete
                                  value={null}
                                  onChange={async (_, user) => {
                                    if (user) {
                                      await handleAddParticipant(
                                        user,
                                        globalPosition,
                                        raidGroup.raidNumber
                                      );
                                    }
                                  }}
                                  options={availableUsers}
                                  getOptionLabel={(user) =>
                                    formatProfileName(user)
                                  }
                                  loading={usersLoading}
                                  renderInput={(params) => (
                                    <TextField
                                      {...params}
                                      variant="outlined"
                                      size="small"
                                      className={styles.participantInput}
                                      placeholder="Выберите участника"
                                      sx={{
                                        "& .MuiOutlinedInput-notchedOutline": {
                                          borderStyle: "dashed !important",
                                          borderColor:
                                            "rgba(255, 255, 255, 0.5) !important",
                                        },
                                        "&:hover .MuiOutlinedInput-notchedOutline":
                                          {
                                            borderStyle: "dashed !important",
                                            borderColor:
                                              "rgba(255, 255, 255, 0.7) !important",
                                          },
                                        "&.Mui-focused .MuiOutlinedInput-notchedOutline":
                                          {
                                            borderStyle: "dashed !important",
                                            borderColor:
                                              "rgba(255, 255, 255, 0.7) !important",
                                          },
                                      }}
                                      onClick={(e) => e.stopPropagation()}
                                    />
                                  )}
                                  renderOption={(props, user) => {
                                    const { key, ...otherProps } = props;
                                    return (
                                      <li
                                        key={key}
                                        className={styles.optionItem}
                                        {...otherProps}
                                      >
                                        <CustomTypography variant="body2">
                                          {formatProfileName(user)}
                                        </CustomTypography>
                                      </li>
                                    );
                                  }}
                                />
                              </div>
                            ) : (
                              // Пустая ячейка без возможности редактирования
                              <div className={styles.emptyReadOnlyCell}>
                                <CustomTypography
                                  variant="caption"
                                  className={styles.emptyText}
                                >
                                  Пустое место
                                </CustomTypography>
                              </div>
                            )}
                          </div>
                        );
                      }
                    )}
                  </div>
                </div>
              ))}
            </div>
          </div>
        );
      })}

      {canEdit && (
        <div className={styles.uploadZone} onClick={handleAddRaid}>
          <CustomTypography variant="body2">
            Создать новый рейд
          </CustomTypography>
        </div>
      )}
    </div>
  );
};

export default ParticipantGrid;
