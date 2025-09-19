"use client";

import type { SSEMessage } from "@/types/sse.types";

type MessageHandler = (data: SSEMessage) => void;

class SSEClient {
  private eventSource: EventSource | null = null;
  private handlers = new Set<MessageHandler>();
  private channel: string;
  private token: string;
  private retryCount = 0;
  private maxRetries = 3;
  private retryDelay = 1000;
  private retryTimer: NodeJS.Timeout | null = null;

  constructor(channel: string, token: string) {
    this.channel = channel;
    this.token = token;
  }

  subscribe(handler: MessageHandler) {
    this.handlers.add(handler);

    // Если это первый хендлер, создаем соединение
    if (this.handlers.size === 1) {
      this.connect();
    }

    return () => {
      this.handlers.delete(handler);

      // Если хендлеров не осталось, закрываем соединение
      if (this.handlers.size === 0) {
        this.disconnect();
        // Удаляем клиент из глобального реестра
        channelClients.delete(this.channel);
      }
    };
  }

  private connect() {
    if (this.eventSource) return;

    const url = new URL(process.env.NEXT_PUBLIC_CENTRIFUGO_SSE_URL!);
    url.searchParams.append(
      "cf_connect",
      JSON.stringify({
        token: this.token,
        name: "freaks-frontend",
        version: "1.0.0",
        subs: { [this.channel]: {} },
      })
    );

    this.eventSource = new EventSource(url.toString());

    this.eventSource.onopen = () => {
      this.retryCount = 0;
    };

    this.eventSource.onmessage = (event) => {
      // Пропускаем пустые сообщения
      if (!event.data || event.data.trim() === "") {
        return;
      }

      try {
        const data = JSON.parse(event.data);

        // Обрабатываем только publish сообщения для нашего канала
        const messageChannel =
          data.channel || data.pub?.channel || data.publish?.channel;
        if (messageChannel === this.channel) {
          // Уведомляем всех хендлеров
          this.handlers.forEach((handler) => handler(data as SSEMessage));
        }
      } catch (error) {
        console.error("Error parsing SSE message:", error);
      }
    };

    this.eventSource.onerror = (error) => {
      console.error(`SSE error for channel ${this.channel}:`, error);
      this.handleDisconnection();
    };
  }

  private handleDisconnection() {
    this.eventSource?.close();
    this.eventSource = null;

    if (this.retryCount < this.maxRetries) {
      this.retryCount++;
      this.retryTimer = setTimeout(() => {
        this.connect();
      }, this.retryDelay);
    }
  }

  disconnect() {
    if (this.retryTimer) {
      clearTimeout(this.retryTimer);
      this.retryTimer = null;
    }

    this.eventSource?.close();
    this.eventSource = null;
    this.retryCount = 0;
  }

  isConnected() {
    return this.eventSource?.readyState === EventSource.OPEN;
  }
}

// Глобальный реестр клиентов по каналам
const channelClients = new Map<string, SSEClient>();

// Получить или создать клиент для канала
export function getChannelClient(channel: string, token: string): SSEClient {
  if (!channelClients.has(channel)) {
    channelClients.set(channel, new SSEClient(channel, token));
  }
  return channelClients.get(channel)!;
}

// Очистить все соединения (для logout)
export function disconnectAllChannels() {
  channelClients.forEach((client) => {
    client.disconnect();
  });
  channelClients.clear();
}
