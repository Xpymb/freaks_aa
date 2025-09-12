"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@mui/material";
import { Add } from "@mui/icons-material";
import { CustomTypography } from "@/components/ui/CustomTypography";
import { NumberInputField } from "@/components/ui/NumberInput/NumberInput";
import { SingleAutoField, type Option } from "@/components/ui/formInputs/SingleAutocomplete";
import { LootItemDto, RaidLootService, LootGradeType } from "@/domains/raids";
import { useTokens } from "@/store/authTokenStore";
import { addLootSchema, type AddLootFormData } from "./addLootSchema";
import CustomImage from "@/components/ui/CustomImage";
import styles from "./_styles.module.scss";

type Props = {
  raidId: number;
  onLootAdded: () => void;
  prefetchLootItems: LootItemDto[];
};

const AddLootForm = ({ raidId, onLootAdded, prefetchLootItems }: Props) => {
  const { accessToken } = useTokens();

  // Используем префетч данные вместо клиентского запроса
  const lootItems = prefetchLootItems;
  const isLoading = false;

  // Настраиваем React Hook Form с Zod валидацией
  const {
    control,
    handleSubmit,
    reset,
    formState: { isSubmitting, errors },
  } = useForm<AddLootFormData>({
    resolver: zodResolver(addLootSchema),
    defaultValues: {
      quantity: 1,
    },
  });

  // Конвертируем LootItemDto в Option для автокомплита
  const lootOptions: Option<number>[] = 
    lootItems?.map((item) => ({
      value: item.id,
      label: item.name,
      data: item, // Добавляем полные данные предмета
    })) || [];

  // Функция для рендеринга опций в автокомплите
  const renderLootOption = (props: React.HTMLAttributes<HTMLLIElement>, option: Option<number>) => {
    const item = option.data as LootItemDto;
    if (!item) return <li {...props}>{option.label}</li>;

    const getGradeColor = (gradeType: LootGradeType) => {
      switch (gradeType) {
        case LootGradeType.Crude:
        case LootGradeType.Basic:
          return '#9e9e9e';
        case LootGradeType.Rare:
          return '#2196f3';
        case LootGradeType.Epic:
          return '#9c27b0';
        case LootGradeType.Legendary:
          return '#ff9800';
        default:
          return '#9e9e9e';
      }
    };

    return (
      <li {...props} key={option.value}>
        <div className={styles.optionContent}>
          <div className={styles.optionIcon}>
            {item.iconUri ? (
              <div className={styles.optionIconContainer}>
                <CustomImage
                  src={`${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${item.iconUri}`}
                  alt={item.name}
                  fill
                  className={styles.optionImage}
                />
                <img
                  src={`/images/masks/icon_grade${item.gradeType}.png`}
                  alt="Grade mask"
                  className={styles.optionGradeMask}
                  onError={(e) => {
                    // Fallback если маска не найдена
                    e.currentTarget.style.display = 'none';
                  }}
                />
              </div>
            ) : (
              <div 
                className={styles.optionColorIcon}
                style={{ backgroundColor: getGradeColor(item.gradeType) }}
              />
            )}
          </div>
          <div className={styles.optionText}>
            <div className={styles.optionName}>{item.name}</div>
            {item.synthesisExp && (
              <div className={styles.optionSynthesis}>
                Опыт: {item.synthesisExp}
              </div>
            )}
          </div>
        </div>
      </li>
    );
  };

  const onSubmit = async (data: AddLootFormData) => {
    if (!accessToken) return;

    try {
      await RaidLootService.createRaidLoot(accessToken, raidId, {
        lootId: data.lootId,
        quantity: data.quantity,
      });
      
      // Сбрасываем форму
      reset();
      
      // Уведомляем родительский компонент
      onLootAdded();
    } catch (error) {
      console.error("Ошибка при добавлении лута:", error);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className={styles.addLootForm}>
        <div className={styles.formHeader}>
          <CustomTypography variant="h5">Добавить лут</CustomTypography>
        </div>
        
        <div className={styles.formContent}>
          {/* Отображение общих ошибок формы */}
          {Object.keys(errors).length > 0 && (
            <div className={styles.formErrors}>
              {errors.lootId && (
                <div className={styles.errorMessage}>
                  {errors.lootId.message}
                </div>
              )}
              {errors.quantity && (
                <div className={styles.errorMessage}>
                  {errors.quantity.message}
                </div>
              )}
            </div>
          )}
          
          <div className={styles.formRow}>
            <div className={styles.autocompleteWrapper}>
              <SingleAutoField<AddLootFormData, number>
                control={control}
                name="lootId"
                options={lootOptions}
                label="Выберите предмет"
                placeholder="Начните вводить название..."
                loading={isLoading}
                disabled={isSubmitting}
                loadingText="Загрузка..."
                noOptionsText="Предметы не найдены"
                renderOption={renderLootOption}
              />
            </div>
            
            <div className={styles.quantityWrapper}>
              <NumberInputField<AddLootFormData>
                control={control}
                name="quantity"
                min={1}
                max={999}
                label="Количество"
                disabled={isSubmitting}
              />
            </div>
            
            <Button
              type="submit"
              variant="contained"
              disabled={isSubmitting}
              startIcon={<Add />}
              className={styles.addButton}
            >
              {isSubmitting ? "Добавление..." : "Добавить"}
            </Button>
          </div>
        </div>
      </div>
    </form>
  );
};

export default AddLootForm;
