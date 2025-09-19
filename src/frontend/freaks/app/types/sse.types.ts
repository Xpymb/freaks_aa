/**
 * Типы действий, выполняемых над сущностью
 */
export enum EntityActionType {
  Created = 1,
  Updated = 2,
  Deleted = 3,
}

/**
 * Базовое сообщение SSE
 */
export interface BaseSSEMessage {
  ActionType: EntityActionType;
  Payload?: unknown;
}

/**
 * Сообщение об изменении рейда
 */
export interface RaidChangedMessage extends BaseSSEMessage {
  Id: number;
}

/**
 * Сообщение об изменении состава участников рейда
 */
export interface RaidParticipantChangedMessage extends BaseSSEMessage {
  RaidId: number;
}

/**
 * Сообщение об изменении скриншотов рейда
 */
export interface RaidScreenshotChangedMessage extends BaseSSEMessage {
  RaidId: number;
}

/**
 * Сообщение об изменении лута рейда
 */
export interface RaidLootChangedMessage extends BaseSSEMessage {
  RaidId: number;
}

/**
 * Структура SSE сообщения от сервера
 */
export interface SSEMessage {
  channel: string;
  pub: {
    data:
      | RaidChangedMessage
      | RaidParticipantChangedMessage
      | RaidScreenshotChangedMessage
      | RaidLootChangedMessage;
  };
}
