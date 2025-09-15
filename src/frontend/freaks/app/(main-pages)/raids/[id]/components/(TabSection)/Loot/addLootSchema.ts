import { z } from "zod";

export const addLootSchema = z.object({
  lootId: z
    .number({
      message: "Выберите предмет лута"
    })
    .min(1, "Выберите предмет лута"),
  quantity: z
    .number({
      message: "Количество должно быть числом"
    })
    .min(1, "Количество должно быть больше 0")
    .max(999, "Количество не может быть больше 999")
    .int("Количество должно быть целым числом"),
});

export type AddLootFormData = z.infer<typeof addLootSchema>;
