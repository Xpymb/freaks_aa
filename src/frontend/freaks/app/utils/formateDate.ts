import { format, isValid, parseISO } from "date-fns";
import { ru } from "date-fns/locale";

/**
 * Список предустановленных форматов даты
 */
export enum DateFormat {
  /** 23 июня 2025 */
  FULL_DATE = "d MMMM yyyy",

  /** 23 июня 2025 14:35 */
  FULL_DATE_TIME = "d MMMM yyyy HH:mm",

  /** 23.06.2025 */
  SHORT_DATE = "dd.MM.yyyy",

  /** 23.06.25 */
  SHORT_DATE_SHORT_YEAR = "dd.MM.yy",

  /** 2025-06-23 */
  ISO_DATE = "yyyy-MM-dd",

  /** июнь 2025 */
  MONTH_YEAR = "MMMM yyyy",

  /** 23 июня */
  DAY_MONTH = "d MMMM",

  /** понедельник, 23 июня 2025 */
  FULL_WEEKDAY = "EEEE, d MMMM yyyy",

  /** 14:35 */
  TIME_ONLY = "HH:mm",

  /** 14:35:12 */
  TIME_WITH_SECONDS = "HH:mm:ss",
}

/**
 * Форматирует дату в нужном формате с поддержкой русской локали.
 *
 * @param date — строка или объект Date
 * @param formatStr — строка формата или предустановленное значение из enum DateFormat, по умолчанию "d MMMM yyyy"
 * @returns строка форматированной даты или "—" при ошибке
 *
 * @example
 * formatDate("2025-06-23") // "23 июня 2025"
 * formatDate(new Date(), DateFormat.FULL_DATE_TIME) // "23 июня 2025 14:35"
 * formatDate("2025-06-23", DateFormat.SHORT_DATE) // "23.06.2025"
 */
export function formatDate(
  date: string | Date | null | undefined,
  formatStr: DateFormat = DateFormat.FULL_DATE
): string {
  if (!date) return "—";

  const dateObj = typeof date === "string" ? parseISO(date) : date;

  if (!isValid(dateObj)) return "—";

  return format(dateObj, formatStr, { locale: ru });
}
