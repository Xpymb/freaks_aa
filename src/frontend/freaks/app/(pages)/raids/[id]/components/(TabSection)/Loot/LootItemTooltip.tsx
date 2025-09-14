"use client";

import React, { useState, useRef } from "react";
import { Tooltip, Paper } from "@mui/material";
import { LootItemDto, LootGradeType, LootType } from "@/domains/raids";
import CustomImage from "@/components/ui/CustomImage";
import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";

type Props = {
  children: React.ReactElement;
  lootItem: LootItemDto;
};

const LootItemTooltip = ({ children, lootItem }: Props) => {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const open = Boolean(anchorEl);
  const timeoutRef = useRef<NodeJS.Timeout | null>(null);

  const handleMouseEnter = (event: React.MouseEvent<HTMLElement>) => {
    // Очищаем предыдущий таймаут
    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }
    
    // Небольшая задержка перед показом
    timeoutRef.current = setTimeout(() => {
      setAnchorEl(event.currentTarget);
    }, 300);
  };

  const handleMouseLeave = () => {
    // Очищаем таймаут если мышь ушла до показа
    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }
    setAnchorEl(null);
  };

  const getGradeName = (gradeType: LootGradeType) => {
    switch (gradeType) {
      case LootGradeType.Crude:
        return "Бесполезный предмет";
      case LootGradeType.Basic:
        return "Обычный предмет";
      case LootGradeType.Grand:
        return "Необычный предмет";
      case LootGradeType.Rare:
        return "Редкий предмет";
      case LootGradeType.Arcane:
        return "Уникальный предмет";
      case LootGradeType.Heroic:
        return "Эпический предмет";
      case LootGradeType.Unique:
        return "Легендарный предмет";
      case LootGradeType.Celestial:
        return "Реликвия";
      case LootGradeType.Divine:
        return "Предмет эпохи чудес";
      case LootGradeType.Epic:
        return "Предмет эпохи сказаний";
      case LootGradeType.Legendary:
        return "Предмет эпохи легенд";
      case LootGradeType.Mythic:
        return "Предмет эпохи мифов";
      case LootGradeType.Eternal:
        return "Предмет эпохи Двенадцати";
      default:
        return "Неизвестный";
    }
  };

  const getTypeName = (type: LootType) => {
    switch (type) {
      case LootType.WorldBossInfusion:
        return "Эссенция мирового босса";
      case LootType.ErenorInfusion:
        return "Эфенская эссенция";
      case LootType.ArcheumCrystal:
        return "Акхиумный кристалл";
      case LootType.WorldBossEquipment:
        return "Экипировка мирового босса";
      case LootType.BossHearth:
        return "Сердце босса";
      case LootType.Lunagem:
        return "Гравировка";
      case LootType.AwakeningScroll:
        return "Свиток пробуждения экипировки мирового босса";
      case LootType.DragonHeartshard:
        return "Генетический материал";
      case LootType.Glider:
        return "Глайдер";
      case LootType.Gold:
        return "Золото";
      case LootType.Other:
        return "Другое";
      default:
        return "Неизвестный тип";
    }
  };

  const getGradeColor = (gradeType: LootGradeType) => {
    switch (gradeType) {
      case LootGradeType.Crude:
        return '#9e9e9e'; // Серый
      case LootGradeType.Basic:
        return '#ffffff'; // Белый
      case LootGradeType.Grand:
        return '#4caf50'; // Зеленый
      case LootGradeType.Rare:
        return '#2196f3'; // Синий
      case LootGradeType.Arcane:
        return '#9c27b0'; // Фиолетовый
      case LootGradeType.Heroic:
        return '#ff5722'; // Оранжевый
      case LootGradeType.Unique:
        return '#ff9800'; // Темно-оранжевый
      case LootGradeType.Celestial:
        return '#00bcd4'; // Голубой
      case LootGradeType.Divine:
        return '#e91e63'; // Розовый
      case LootGradeType.Epic:
        return '#9c27b0'; // Фиолетовый
      case LootGradeType.Legendary:
        return '#ff9800'; // Оранжевый
      case LootGradeType.Mythic:
        return '#f44336'; // Красный
      case LootGradeType.Eternal:
        return '#ffeb3b'; // Желтый
      default:
        return '#9e9e9e';
    }
  };

  return (
    <Tooltip
      open={open}
      title={
        <Paper 
          className={styles.tooltipContent}
          style={{ borderColor: getGradeColor(lootItem.gradeType) }}
        >
          <div className={styles.tooltipHeader}>
            <div className={styles.tooltipIcon}>
              {lootItem.iconUri ? (
                <div className={styles.tooltipIconContainer}>
                  <CustomImage
                    src={`${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${lootItem.iconUri}`}
                    alt={lootItem.name}
                    fill
                    className={styles.tooltipImage}
                  />
                  <CustomImage
                    src={`/images/masks/icon_grade${lootItem.gradeType}.png`}
                    alt="Grade mask"
                    fill
                    className={styles.tooltipGradeMask}
                  />
                </div>
              ) : (
                <div 
                  className={styles.tooltipColorIcon}
                  style={{ backgroundColor: getGradeColor(lootItem.gradeType) }}
                />
              )}
            </div>
            
            <div className={styles.tooltipInfo}>
              <CustomTypography 
                variant="caption" 
                className={styles.tooltipType}
              >
                {getTypeName(lootItem.type)}
              </CustomTypography>
              
              <CustomTypography 
                variant="body2" 
                className={styles.tooltipRarity}
                style={{ color: getGradeColor(lootItem.gradeType) }}
              >
                {getGradeName(lootItem.gradeType)}
              </CustomTypography>
              
              <CustomTypography 
                variant="h6" 
                className={styles.tooltipName}
                style={{ color: getGradeColor(lootItem.gradeType) }}
              >
                {lootItem.name}
              </CustomTypography>
            </div>
          </div>
          
          <div className={styles.tooltipBody}>
            {lootItem.synthesisExp && (
              <div className={styles.tooltipProperty}>
                <CustomTypography variant="caption" className={styles.propertyLabel}>
                  Опыт:
                </CustomTypography>
                <CustomTypography variant="caption" className={styles.propertyValue}>
                  {lootItem.synthesisExp}
                </CustomTypography>
              </div>
            )}
            
            {lootItem.description && (
              <div className={styles.tooltipDescription}>
                <CustomTypography variant="caption" className={styles.descriptionText}>
                  {lootItem.description}
                </CustomTypography>
              </div>
            )}
          </div>
        </Paper>
      }
      placement="right"
      arrow
      PopperProps={{
        onMouseEnter: () => {}, // Оставляем тултип открытым при наведении на него
        onMouseLeave: handleMouseLeave,
      }}
    >
      <div
        onMouseEnter={handleMouseEnter}
        onMouseLeave={handleMouseLeave}
      >
        {children}
      </div>
    </Tooltip>
  );
};

export default LootItemTooltip;
