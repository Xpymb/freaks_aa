"use client";

import React, { useState, useEffect, useMemo } from "react";
import { Autocomplete, TextField, IconButton, Button } from "@mui/material";
import { Close as CloseIcon, Add as AddIcon } from "@mui/icons-material";
import { RaidParticipantDto } from "@/domains/raids";
import { IUser } from "@/types/user.types";
import { useRaidParticipantMutations } from "@/domains/raids/hooks/useRaidParticipantMutations";
import { useGetUsers } from "@/domains/user/hooks/useGetUsers";
import { formatProfileName } from "@/utils/formatProfileName";
import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";

type Props = {
  raidId: number;
  participants: RaidParticipantDto[];
  prefetchUsers: IUser[];
  onParticipantsChange: () => void;
};

const GRID_SIZE = 50; // 10x5 grid
const POSITIONS_PER_PARTY = 5;

const ParticipantGrid = ({ raidId, participants, prefetchUsers, onParticipantsChange }: Props) => {
  const [inputValues, setInputValues] = useState<Record<number, string>>({});
  const [hasChanges, setHasChanges] = useState(false);
  const [raids, setRaids] = useState([{ id: 1, participants: participants }]);
  const { createParticipant, deleteParticipant } = useRaidParticipantMutations(raidId);
  const { data: users = prefetchUsers, isLoading: usersLoading } = useGetUsers(prefetchUsers);

  // Создаем сетку участников
  const gridData = useMemo(() => {
    const grid = new Array(GRID_SIZE).fill(null);
    
    participants.forEach((participant) => {
      const position = (participant.raidPartyNumber - 1) * POSITIONS_PER_PARTY + 
                      (participant.raidPartyPositionNumber - 1);
      if (position >= 0 && position < GRID_SIZE) {
        grid[position] = participant;
      }
    });
    
    return grid;
  }, [participants]);

  // Обработчики для добавления участника
  const handleAddParticipant = async (user: IUser, position: number) => {
    const partyNumber = Math.floor(position / POSITIONS_PER_PARTY) + 1;
    const positionInParty = (position % POSITIONS_PER_PARTY) + 1;
    
    try {
      await createParticipant.trigger({
        participantId: user.id,
        raidNumber: 1, // Пока всегда 1, можно будет сделать настраиваемым
        raidPartyNumber: partyNumber,
        raidPartyPositionNumber: positionInParty,
      });
      
      // Очищаем инпут после успешного добавления
      setInputValues(prev => {
        const newValues = { ...prev };
        delete newValues[position];
        return newValues;
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


  // Обработчик изменения инпута
  const handleInputChange = (position: number, value: string) => {
    setInputValues(prev => ({
      ...prev,
      [position]: value
    }));
    setHasChanges(true);
  };

  // Обработчик сохранения изменений
  const handleSave = async () => {
    try {
      // Сначала удаляем всех существующих участников
      for (const participant of participants) {
        await deleteParticipant.trigger(participant.participant.id);
      }

      // Затем добавляем новых участников из инпутов
      for (const [positionStr, nickname] of Object.entries(inputValues)) {
        if (nickname.trim()) {
          const position = parseInt(positionStr);
          const user = users.find(u => 
            u.gameNickname?.toLowerCase().includes(nickname.toLowerCase()) ||
            u.username?.toLowerCase().includes(nickname.toLowerCase())
          );
          
          if (user) {
            const partyNumber = Math.floor(position / POSITIONS_PER_PARTY) + 1;
            const positionInParty = (position % POSITIONS_PER_PARTY) + 1;
            
            await createParticipant.trigger({
              participantId: user.id,
              raidNumber: 1,
              raidPartyNumber: partyNumber,
              raidPartyPositionNumber: positionInParty,
            });
          }
        }
      }

      setHasChanges(false);
      setInputValues({});
      onParticipantsChange();
    } catch (error) {
      console.error("Failed to save participants:", error);
    }
  };

  // Обработчик сброса изменений
  const handleReset = () => {
    setInputValues({});
    setHasChanges(false);
  };

  // Добавить новый рейд
  const handleAddRaid = () => {
    const newRaidId = raids.length + 1;
    setRaids(prev => [...prev, { id: newRaidId, participants: [] }]);
  };

  // Удалить рейд
  const handleRemoveRaid = (raidIndex: number) => {
    if (raids.length > 1) {
      setRaids(prev => prev.filter((_, index) => index !== raidIndex));
    }
  };

  // Фильтруем пользователей, исключая уже добавленных
  const availableUsers = useMemo(() => {
    const addedUserIds = new Set(participants.map(p => p.participant.id));
    return users.filter(user => !addedUserIds.has(user.id));
  }, [users, participants]);

  // Обработчик изменения участников
  useEffect(() => {
    const handleRefresh = () => {
      onParticipantsChange();
    };

    window.addEventListener('refresh-participants', handleRefresh);
    return () => {
      window.removeEventListener('refresh-participants', handleRefresh);
    };
  }, [onParticipantsChange]);

  return (
    <div className={styles.participantGrid}>
      {raids.map((raid, raidIndex) => (
        <div key={raid.id} className={styles.raidContainer}>
          <div className={styles.raidHeader}>
            <CustomTypography variant="h6" className={styles.raidTitle}>
              Рейд {raid.id}
            </CustomTypography>
            {raids.length > 1 && (
              <IconButton
                size="small"
                onClick={() => handleRemoveRaid(raidIndex)}
                className={styles.removeRaidButton}
              >
                <CloseIcon fontSize="small" />
              </IconButton>
            )}
          </div>

          <div className={styles.gridContainer}>
        {Array.from({ length: 5 }, (_, partyIndex) => (
          <div key={partyIndex} className={styles.party}>
            <CustomTypography variant="h6" className={styles.partyLabel}>
              Отряд {partyIndex + 1}
            </CustomTypography>
            <div className={styles.partyGrid}>
              {Array.from({ length: POSITIONS_PER_PARTY }, (_, positionIndex) => {
                const globalPosition = partyIndex * POSITIONS_PER_PARTY + positionIndex;
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
                        <CustomTypography variant="subtitle1" className={styles.participantName}>
                          {formatProfileName(participant.participant)}
                        </CustomTypography>
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
                      </div>
                    ) : (
                      <div className={styles.cellAutocomplete}>
                        <Autocomplete
                          value={inputValues[globalPosition] ? users.find(u => 
                            u.gameNickname?.toLowerCase().includes(inputValues[globalPosition].toLowerCase()) ||
                            u.username?.toLowerCase().includes(inputValues[globalPosition].toLowerCase())
                          ) || null : null}
                          onChange={async (_, user) => {
                            if (user) {
                              handleInputChange(globalPosition, formatProfileName(user));
                              // Автоматически добавляем участника
                              await handleAddParticipant(user, globalPosition);
                            } else {
                              handleInputChange(globalPosition, '');
                            }
                          }}
                          options={availableUsers}
                          getOptionLabel={(user) => formatProfileName(user)}
                          loading={usersLoading}
                           renderInput={(params) => (
                             <TextField
                               {...params}
                               variant="outlined"
                               size="small"
                               className={styles.participantInput}
                               sx={{
                                 '& .MuiOutlinedInput-notchedOutline': {
                                   borderStyle: 'dashed !important',
                                   borderColor: 'rgba(255, 255, 255, 0.5) !important'
                                 },
                                 '&:hover .MuiOutlinedInput-notchedOutline': {
                                   borderStyle: 'dashed !important',
                                   borderColor: 'rgba(255, 255, 255, 0.7) !important'
                                 },
                                 '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                                   borderStyle: 'solid !important',
                                   borderColor: 'rgba(255, 255, 255, 0.8) !important'
                                 }
                               }}
                               onClick={(e) => e.stopPropagation()}
                             />
                           )}
                          renderOption={(props, user) => {
                            const { key, ...otherProps } = props;
                            return (
                              <li key={key} className={styles.optionItem} {...otherProps}>
                                <CustomTypography variant="body2">
                                  {formatProfileName(user)}
                                </CustomTypography>
                              </li>
                            );
                          }}
                          onInputChange={(_, value) => {
                            handleInputChange(globalPosition, value);
                          }}
                        />
                      </div>
                    )}
                  </div>
                );
              })}
            </div>
          </div>
        ))}
        {Array.from({ length: 5 }, (_, partyIndex) => {
          const actualPartyIndex = partyIndex + 5; // Отряды 6-10
          return (
            <div key={actualPartyIndex} className={styles.party}>
              <CustomTypography variant="h6" className={styles.partyLabel}>
                Отряд {actualPartyIndex + 1}
              </CustomTypography>
              <div className={styles.partyGrid}>
                {Array.from({ length: POSITIONS_PER_PARTY }, (_, positionIndex) => {
                  const globalPosition = actualPartyIndex * POSITIONS_PER_PARTY + positionIndex;
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
                          <CustomTypography variant="subtitle1" className={styles.participantName}>
                            {formatProfileName(participant.participant)}
                          </CustomTypography>
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
                        </div>
                      ) : (
                        <div className={styles.cellAutocomplete}>
                          <Autocomplete
                            value={inputValues[globalPosition] ? users.find(u => 
                              u.gameNickname?.toLowerCase().includes(inputValues[globalPosition].toLowerCase()) ||
                              u.username?.toLowerCase().includes(inputValues[globalPosition].toLowerCase())
                            ) || null : null}
                            onChange={async (_, user) => {
                              if (user) {
                                handleInputChange(globalPosition, formatProfileName(user));
                                // Автоматически добавляем участника
                                await handleAddParticipant(user, globalPosition);
                              } else {
                                handleInputChange(globalPosition, '');
                              }
                            }}
                            options={availableUsers}
                            getOptionLabel={(user) => formatProfileName(user)}
                            loading={usersLoading}
                            renderInput={(params) => (
                              <TextField
                                {...params}
                                variant="outlined"
                                size="small"
                                className={styles.participantInput}
                                sx={{
                                  '& .MuiOutlinedInput-notchedOutline': {
                                    borderStyle: 'dashed !important',
                                    borderColor: 'rgba(255, 255, 255, 0.5) !important'
                                  },
                                  '&:hover .MuiOutlinedInput-notchedOutline': {
                                    borderStyle: 'dashed !important',
                                    borderColor: 'rgba(255, 255, 255, 0.7) !important'
                                  },
                                  '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                                    borderStyle: 'solid !important',
                                    borderColor: 'rgba(255, 255, 255, 0.8) !important'
                                  }
                                }}
                                onClick={(e) => e.stopPropagation()}
                              />
                            )}
                            renderOption={(props, user) => {
                              const { key, ...otherProps } = props;
                              return (
                                <li key={key} className={styles.optionItem} {...otherProps}>
                                  <CustomTypography variant="body2">
                                    {formatProfileName(user)}
                                  </CustomTypography>
                                </li>
                              );
                            }}
                            onInputChange={(_, value) => {
                              handleInputChange(globalPosition, value);
                            }}
                          />
                        </div>
                      )}
                    </div>
                  );
                })}
              </div>
            </div>
          );
        })}
          </div>
        </div>
      ))}

      <div className={styles.addRaidContainer}>
        <Button
          variant="outlined"
          startIcon={<AddIcon />}
          onClick={handleAddRaid}
          className={styles.addRaidButton}
        >
          Добавить рейд
        </Button>
      </div>
    </div>
  );
};

export default ParticipantGrid;
