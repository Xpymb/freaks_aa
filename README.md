# Freaks ArcheAge Guild CRM

<div align="center">

![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![Next.js](https://img.shields.io/badge/Next.js-16.1-black?logo=next.js)
![React](https://img.shields.io/badge/React-19.1-61DAFB?logo=react)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

[Начало работы](#-начало-работы) • [Архитектура](#-архитектура) • [Функции](#-основные-функции) • [Разработка](#%EF%B8%8F-разработка) • [Deployment](#-deployment)

</div>

## О проекте

**Freaks AA** — комплексная CRM-система для управления гильдией в MMORPG **ArcheAge**.

Платформа построена на современной микросервисной архитектуре и предоставляет инструменты для:
- 🎮 Управления рейдами с отслеживанием участников, лута и скриншотов
- 💰 Автоматизированного расчета зарплат членов гильдии
- 🏪 Аукционной системы и магазина гильдии
- 📢 Многоканальных уведомлений (Web, Discord, Telegram)
- 👥 Управления пользователями с role-based доступом
- 🤖 Интеграции с Discord и Telegram ботами

### Технологический стек

| Категория | Технологии |
|-----------|-----------|
| **Backend** | .NET 10.0, ASP.NET Core 10.0.1, EF Core 10.0.1 |
| **Frontend** | Next.js 16.1, React 19.1, TypeScript 5, Material-UI 7.3 |
| **Базы данных** | PostgreSQL 16, Redis |
| **Messaging** | RabbitMQ, Centrifugo (WebSocket) |
| **Storage** | MinIO (S3-совместимое) |
| **Auth** | Keycloak (OAuth2/OpenID Connect) |
| **State Management** | Zustand 5.0.8 |
| **Data Fetching** | SWR 2.3.4, Axios 1.12 |
| **Forms** | React Hook Form 7.61.1, Zod 4.0.10 |
| **Deployment** | Docker, Kubernetes, Werf, Helm |
| **CI/CD** | GitHub Actions |

---

## 🏗 Архитектура

### Обзор системы

```
┌─────────────────────────────────────────────────────────────┐
│                    Клиентский уровень                         │
├───────────────────┬─────────────────┬───────────────────────┤
│  Next.js Frontend │   Discord Bot   │   Telegram Bot        │
└─────────┬─────────┴────────┬────────┴──────────┬────────────┘
          │                  │                   │
          └──────────────────┴───────────────────┘
                             │
          ┌──────────────────┴──────────────────┐
          │      API Gateway (Keycloak Auth)    │
          └──────────────────┬──────────────────┘
                             │
     ┌───────────────────────┼────────────────────────┐
     │                       │                        │
┌────▼─────┐          ┌─────▼──────┐          ┌─────▼──────┐
│  Portal  │          │   Files    │          │    Bots    │
│  Service │          │  Service   │          │  Services  │
└────┬─────┘          └─────┬──────┘          └─────┬──────┘
     │                      │                        │
     └──────────────────────┴────────────────────────┘
                            │
     ┌──────────────────────┴────────────────────────┐
     │                                                │
┌────▼────┐  ┌────────┐  ┌─────────┐  ┌──────────┐ │
│PostgreSQL│  │ Redis  │  │RabbitMQ │  │  MinIO   │ │
└─────────┘  └────────┘  └─────────┘  └──────────┘ │
                                                     │
                                          ┌──────────▼┐
                                          │ Centrifugo│
                                          └───────────┘
```

### Микросервисная структура

#### Portal Service (основной сервис)
**Проектов:** 14
**Назначение:** Управление рейдами, зарплатами, аукционами, магазином и уведомлениями

**Доменные области:**
- `RaidSummary` - управление рейдами, участниками, лутом, скриншотами
- `SalarySummary` - автоматический расчет зарплат, управление расходами
- `Auction` - аукционная система с системой ставок
- `Shop` - магазин гильдии с заявками
- `Loot` - управление предметами добычи
- `Notification` - система уведомлений через разные каналы

**Архитектура:** WebAPI → BLL (Interfaces/Implementation) → DAL (Interfaces/Implementation/Persistence) → PostgreSQL

#### Files Service
**Проектов:** 7
**Назначение:** Управление файлами (скриншоты, изображения, документы)

**Технологии:** MinIO (S3-совместимое хранилище), Factory Pattern для обработки разных типов файлов

#### Bot Services
- **Discord Bot** (7 проектов) - уведомления и команды в Discord
- **Telegram Bot** (7 проектов) - уведомления и команды в Telegram

#### Shared Libraries (17 проектов)
Общие компоненты для всех микросервисов:
- `Freaks.Bll.Common` - базовые классы бизнес-логики
- `Freaks.Dal.Common` - базовые провайдеры доступа к данным
- `Freaks.WebApi.Common` - общие веб-компоненты, middleware, фильтры
- `Freaks.Messages.*` (4 проекта) - система real-time сообщений через Centrifugo
- `Freaks.Users.*` (5 проектов) - управление пользователями и интеграция с Keycloak

### Слои приложения (Clean Architecture)

```
┌──────────────────────────────────────┐
│         WebAPI Layer                 │  ← Controllers, Swagger, Middleware
├──────────────────────────────────────┤
│         BLL Layer                    │  ← Business Logic, Services, Algorithms
├──────────────────────────────────────┤
│         DAL Layer                    │  ← Providers, Repositories, DbContext
├──────────────────────────────────────┤
│         Database Layer               │  ← PostgreSQL, Миграции
└──────────────────────────────────────┘
```

---

## 📁 Структура проекта

```
freaks_aa/
├── src/
│   ├── backend/dotnet/                        # Backend .NET 10.0 (45 проектов)
│   │   ├── Services/                          # Микросервисы
│   │   │   ├── Portal/                        # Portal Service (14 проектов)
│   │   │   │   ├── Bll/
│   │   │   │   │   ├── Freaks.Portal.Bll.Interfaces/
│   │   │   │   │   └── Freaks.Portal.Bll.Implementation/
│   │   │   │   ├── Dal/
│   │   │   │   │   ├── Freaks.Portal.Dal.Interfaces/
│   │   │   │   │   ├── Freaks.Portal.Dal.Implementation/
│   │   │   │   │   └── Freaks.Portal.Dal.Persistence/   # DbContext, Migrations
│   │   │   │   ├── Freaks.Portal.WebApi/
│   │   │   │   ├── Freaks.Portal.Contracts/
│   │   │   │   └── Freaks.Portal.SharedContracts/
│   │   │   ├── Files/                         # Files Service (7 проектов)
│   │   │   │   ├── Bll/, Dal/
│   │   │   │   └── Freaks.Files.WebApi/
│   │   │   └── Bots/                          # Bot Services (14 проектов)
│   │   │       ├── DiscordBot/                # Discord интеграция (7 проектов)
│   │   │       └── TelegramBot/               # Telegram интеграция (7 проектов)
│   │   └── Shared/                            # Shared Libraries (17 проектов)
│   │       ├── Freaks.Bll.Common/
│   │       ├── Freaks.Dal.Common/
│   │       ├── Freaks.WebApi.Common/
│   │       ├── Freaks.Contracts.Common/
│   │       ├── Freaks.Options.Common/
│   │       ├── Freaks.SharedContracts.Common/
│   │       ├── Messages/                      # Система сообщений (4 проекта)
│   │       │   ├── Freaks.Messages.Bll/
│   │       │   ├── Freaks.Messages.Centrifugo/
│   │       │   └── Freaks.Messages.Common/
│   │       └── Users/                         # Пользователи (5 проектов)
│   │           ├── Freaks.Users.Bll/
│   │           ├── Freaks.Users.Dal/
│   │           └── Freaks.Users.Contracts/
│   │
│   └── frontend/freaks/                       # Next.js 16.1 приложение
│       ├── app/                               # App Router
│       │   ├── (main-pages)/                  # Защищенные страницы
│       │   │   ├── overview/                  # Главная страница
│       │   │   ├── raids/                     # Управление рейдами
│       │   │   ├── reports/                   # Отчеты (зарплаты)
│       │   │   └── admin-panel/               # Панель администратора
│       │   ├── (help-pages)/                  # Публичные страницы
│       │   │   ├── login/, logout/, forbidden/
│       │   ├── api/                           # API routes (NextAuth)
│       │   ├── domains/                       # Доменная логика
│       │   │   ├── auth/, raids/, reports/
│       │   │   ├── loot/, files/, user/
│       │   ├── components/                    # React компоненты
│       │   ├── store/                         # Zustand state management
│       │   ├── shared/                        # API клиенты, utilities
│       │   └── ThemeMUI/                      # Material-UI темизация
│       ├── middleware.ts                      # Auth middleware
│       ├── package.json
│       └── next.config.ts
│
├── docker/                                    # Docker конфигурация
│   ├── docker-compose-local-back.yml          # Полный backend stack
│   ├── docker-compose-local-db.yml            # Только базы данных
│   ├── docker-compose-prod.yml                # Production конфигурация
│   ├── Postgres/, Redis/, RabbitMQ/
│   ├── MinIO/, Centrifugo/
│   └── dotnet/, frontend/                     # Dockerfiles сервисов
│
├── .helm/                                     # Helm charts для Kubernetes
│   ├── Chart.yaml
│   └── charts/
│       ├── portal-webapi/
│       ├── files-webapi/
│       └── frontend/
│
├── .github/workflows/                         # CI/CD
│   └── build-and-deploy-staging.yml
│
├── werf.yaml                                  # Werf конфигурация для deployment
├── README.md
└── LICENSE
```

---

## 🚀 Начало работы

### Системные требования

- **Операционная система:** Windows 10+, macOS 12+, Linux
- **.NET SDK:** 10.0 или выше ([скачать](https://dotnet.microsoft.com/download))
- **Node.js:** 22.x LTS ([скачать](https://nodejs.org/))
- **Docker:** 20.10+ ([скачать](https://www.docker.com/get-started))
- **Docker Compose:** 2.0+
- **Git:** 2.30+

### 1. Клонирование репозитория

```bash
git clone https://github.com/Xpymb/freaks_aa.git
cd freaks_aa
```

### 2. Подготовка окружения

The project uses a .env file stored in the root of the docker/ folder. To initialize it:

```bash
cd docker
./setup-env.sh        # for Linux/macOS
setup-env.cmd         # for Windows
```

This script will:
- Copy .env.local to .env
- Prompt you to enter your KEYCLOAK_ADMIN_CLIENT_SECRET

### 3. Запуск сервисов

There are convenience scripts for starting the environment:

**Run full backend:**
```bash
./run_docker-compose-local-back.sh    # for Linux/macOS
run_docker-compose-local-back.cmd     # for Windows
```

**Run databases only:**
```bash
./run_docker-compose-local-db.sh     # for Linux/macOS
run_docker-compose-local-db.cmd      # for Windows
```

### 4. Проверка работоспособности

После запуска сервисов проверьте доступность компонентов:

**Backend API:**
- Portal Service: http://localhost:5100/swagger
- Files Service: http://localhost:5110/swagger

**Frontend:**
- Приложение: http://localhost:3000

**Инфраструктура:**
- Keycloak: https://auth.freaks-aa.ru/
- RabbitMQ Management: http://localhost:15672 (guest/guest)
- MinIO Console: http://localhost:9001

### 5. Первый вход в систему

1. Откройте http://localhost:3000
2. Нажмите кнопку "Войти"
3. Вы будете перенаправлены на Keycloak
4. Используйте учетные данные администратора
5. После авторизации откроется главная страница CRM

---

## 📦 Зависимости

- **.NET SDK:** 10.0 или выше
- **Node.js:** 22.x LTS (для frontend разработки)
- **Docker + Docker Compose:** для локального запуска инфраструктуры
- **Keycloak:** работает через Docker
- **PostgreSQL, Redis, RabbitMQ, MinIO:** включены в docker-compose

---

## 🔐 Аутентификация

Система использует **Keycloak** как Identity Provider с поддержкой OAuth 2.0 / OpenID Connect.

**Frontend:** Next-Auth 5.0 с Authorization Code Flow
**Backend:** JWT Bearer Token валидация

После запуска Keycloak будет доступен по адресу:
```
https://auth.freaks-aa.ru/
```

Swagger UI предварительно настроен на OAuth2 авторизацию через Keycloak.

---

## ✨ Основные функции

### 🎮 Управление рейдами
- Создание и планирование рейдов на боссов ArcheAge
- Управление списком участников рейда
- Отслеживание лута с привязкой к игрокам
- Загрузка и просмотр скриншотов рейда
- Контроль доступа на основе ролей (Guild Leader, Officer, Member)
- Статусы рейдов: Запланирован → Ожидание скриншотов → Ожидание подтверждения → Завершен

### 💰 Система зарплат
- Автоматический расчет зарплат на основе активности в рейдах
- Настраиваемый алгоритм расчета (BasicSalaryAlgorithm)
- Учет процента активности каждого участника
- Система дисконтов и дополнительных выплат (discount/amount)
- Управление расходами гильдии (SalaryExpenses)
- Детальные отчеты по зарплатам за период
- Многошаговое заполнение отчетов (Stepper UI)

### 🏪 Аукционная система
- Создание внутригильдейских аукционов предметов
- Система ставок с автоматическим определением победителя
- История всех торгов и ставок
- Интеграция с системой предметов (LootItems)

### 🛒 Магазин гильдии
- Каталог доступных предметов для покупки
- Система заявок на покупку (ShopItemRequest)
- Управление запасами предметов
- История покупок участников

### 📢 Система уведомлений
- Многоканальные уведомления (Web, Discord, Telegram)
- Настраиваемые каналы уведомлений
- Real-time обновления через Centrifugo WebSocket/SSE
- Шаблоны сообщений для разных событий

### 👥 Управление пользователями
- Полная интеграция с Keycloak
- Role-based access control (Admin, Guild Leader, Officer, Member)
- Профили пользователей с игровыми никнеймами
- Синхронизация ролей между системой и Keycloak

### 🤖 Discord и Telegram боты
- Автоматические уведомления о событиях гильдии
- Команды для взаимодействия с CRM
- Синхронизация событий из рейдов
- Двусторонняя интеграция с основной системой

---

## ⚒️ Разработка

### Backend разработка (.NET 10)

#### Архитектура сервиса

Каждый микросервис следует единой архитектуре с разделением слоев:

1. **WebAPI проект** - Entry point, Controllers, Middleware, Swagger
2. **BLL.Interfaces** - Интерфейсы бизнес-логики (сервисы)
3. **BLL.Implementation** - Реализация бизнес-логики
4. **DAL.Interfaces** - Интерфейсы провайдеров доступа к данным
5. **DAL.Implementation** - Реализация провайдеров
6. **DAL.Persistence** - EF Core DbContext, Entity Configurations, Migrations
7. **Contracts** - DTO для внешнего API
8. **SharedContracts** - DTO для межсервисного взаимодействия

#### Локальный запуск backend сервиса

```bash
# Запустить только Portal Service (без Docker)
cd src/backend/dotnet/Services/Portal/Freaks.Portal.WebApi
dotnet run

# Swagger будет доступен на http://localhost:5100/swagger
```

#### Работа с миграциями EF Core

```bash
# Создать новую миграцию для Portal Service
cd src/backend/dotnet
dotnet ef migrations add MigrationName \
  --project Services/Portal/Dal/Freaks.Portal.Dal.Persistence \
  --startup-project Services/Portal/Freaks.Portal.WebApi

# Применить миграции
dotnet ef database update \
  --project Services/Portal/Dal/Freaks.Portal.Dal.Persistence \
  --startup-project Services/Portal/Freaks.Portal.WebApi

# Откатить последнюю миграцию
dotnet ef database update PreviousMigrationName \
  --project Services/Portal/Dal/Freaks.Portal.Dal.Persistence \
  --startup-project Services/Portal/Freaks.Portal.WebApi
```

#### Добавление нового API endpoint

1. Создать Entity в `Contracts/Entities`
2. Добавить Entity в `DbContext` (Dal.Persistence)
3. Создать интерфейс провайдера в `Dal.Interfaces`
4. Реализовать провайдер в `Dal.Implementation`
5. Создать интерфейс сервиса в `Bll.Interfaces`
6. Реализовать сервис в `Bll.Implementation`
7. Создать контроллер в `WebApi/Controllers`
8. Зарегистрировать в DI (обычно автоматически через `ConfigureServices.cs`)

#### Используемые паттерны

- **Provider Pattern** - DAL использует провайдеров вместо репозиториев
- **Service Pattern** - BLL содержит бизнес-логику в сервисах
- **Factory Pattern** - FileService использует фабрику
- **Unit of Work** - через EF Core DbContext
- **Dependency Injection** - все компоненты регистрируются в DI

---

### Frontend разработка (Next.js 16)

#### Структура и паттерны

- **App Router** - Next.js 13+ file-based routing
- **Server Components** - по умолчанию для всех компонентов
- **Client Components** - только где необходима интерактивность (`"use client"`)
- **Domain-Driven Design** - логика разбита по доменам в `/app/domains`

#### Локальный запуск frontend

```bash
cd src/frontend/freaks

# Установить зависимости
npm install

# Запустить dev сервер с Turbopack
npm run dev

# Приложение доступно на http://localhost:3000
```

#### Создание нового модуля

1. Создать директорию `app/domains/[module-name]/`
2. Создать сервисный файл `[module].service.ts` с API методами
3. Создать типы в `types.ts`
4. Создать custom hooks в `hooks/use[Module].ts`
5. Создать компоненты страниц в `app/(main-pages)/[module]/`
6. Обновить middleware.ts для защиты маршрутов (если нужно)

#### Работа с API (примеры)

```typescript
// Использование SWR для GET запросов
import { useProtectedSWR } from '@/app/shared/api/useProtectedSWR';

const { data, error, isLoading } = useProtectedSWR<Raid[]>('/api/raids');

// Использование authorizedApi для POST/PUT/DELETE
import { authorizedApi } from '@/app/shared/api/authorizedApi';

const createRaid = async (data: CreateRaidRequest) => {
  const token = await getAccessToken();
  return authorizedApi.post<Raid>(token, '/raids', data);
};
```

#### State Management (Zustand)

```typescript
import { create } from 'zustand';

export const useRaidStore = create<RaidStore>((set) => ({
  selectedRaid: null,
  setSelectedRaid: (raid) => set({ selectedRaid: raid }),
}));
```

#### Стилизация

- **Material-UI** - основные компоненты
- **Emotion** - CSS-in-JS для кастомных стилей
- **SCSS Modules** - для компонентов специфичных стилей
- **Dark Theme** - по умолчанию (настроено в ThemeMUI/)

---

### Environment Variables

#### Backend (.NET)

Конфигурация через `appsettings.json` и переменные окружения:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=freaks;..."
  },
  "Keycloak": {
    "Authority": "https://auth.freaks-aa.ru/realms/freaks-dev"
  }
}
```

В production переменные передаются через Kubernetes Secrets.

#### Frontend (Next.js)

Создать `.env.local` в `src/frontend/freaks/`:

```bash
NEXTAUTH_URL=http://localhost:3000
NEXTAUTH_SECRET=your-secret-key-here
NEXT_PUBLIC_API_URL=http://localhost:5100
NEXT_PUBLIC_FILES_API_URL=http://localhost:5110
NEXT_PUBLIC_KEYCLOAK_ISSUER=https://auth.freaks-aa.ru/realms/freaks-dev
```

---

### Порты сервисов (локальная разработка)

| Сервис | Порт | Описание |
|--------|------|----------|
| Frontend | 3000 | Next.js приложение |
| Portal API | 5100 | Portal WebAPI + Swagger |
| Files API | 5110 | Files WebAPI + Swagger |
| Discord Bot | 5120 | Discord Bot API |
| Telegram Bot | 5130 | Telegram Bot API |
| PostgreSQL | 5432 | База данных |
| Redis | 6379 | Кеш и сессии |
| RabbitMQ | 5672 | AMQP протокол |
| RabbitMQ Management | 15672 | Web UI (guest/guest) |
| MinIO | 9000 | S3 API |
| MinIO Console | 9001 | Web Console |
| Centrifugo | 8000 | WebSocket/SSE сервер |

---

### Code Style

**Backend (.NET):**
- C# 14 features
- Nullable reference types включены (`<Nullable>enable</Nullable>`)
- Async/await для всех I/O операций
- Dependency Injection для всех зависимостей
- Mapster для маппинга вместо AutoMapper

**Frontend (TypeScript):**
- TypeScript strict mode
- Functional components с hooks (не классовые компоненты)
- ESLint конфигурация Next.js
- Prettier для автоформатирования

---

## 🚀 Deployment

### CI/CD Pipeline

Проект использует **GitHub Actions** + **Werf** для автоматизации сборки и развертывания.

#### Workflow процесс

```
┌──────────────┐      ┌────────────────┐      ┌──────────────┐
│  Git Push    │ ───▶ │ GitHub Actions │ ───▶ │ Werf Build   │
│  to branch   │      │  Workflow      │      │  Images      │
└──────────────┘      └────────────────┘      └──────┬───────┘
                                                      │
                                                      ▼
                                               ┌──────────────┐
                                               │  Container   │
                                               │  Registry    │
                                               └──────┬───────┘
                                                      │
                                                      ▼
                                               ┌──────────────┐
                                               │  Kubernetes  │
                                               │   Cluster    │
                                               └──────────────┘
```

#### Werf конфигурация (werf.yaml)

Werf собирает следующие образы:

1. **solution-build-artifacts** - базовый образ с скомпилированным .NET решением
2. **portal-webapi** - Portal Service (.NET 9 Alpine)
3. **files-webapi** - Files Service (.NET 9 Alpine)
4. **frontend** - Next.js приложение (Node 22 Alpine)
5. **users-migrations** - миграции для Users schema
6. **portal-webapi-migrations** - миграции для Portal schema

#### Автоматическое развертывание на Staging

Workflow `.github/workflows/build-and-deploy-staging.yml` запускается вручную через `workflow_dispatch`.

**Шаги:**
1. **Build** - собирает Docker образы через Werf
2. **Deploy** - разворачивает в Kubernetes через `werf converge`

```bash
# Ручной запуск deployment
werf converge --env dev
```

#### Helm Charts

Проект использует Helm для управления Kubernetes манифестами в директории `.helm/`:

```
.helm/
├── Chart.yaml              # Главный Helm chart
└── charts/
    ├── portal-webapi/      # Deployment, Service, Ingress для Portal
    ├── files-webapi/       # Deployment, Service, Ingress для Files
    └── frontend/           # Deployment, Service, Ingress для Frontend
```

Каждый chart содержит:
- **Deployment** - описание подов
- **Service** - внутренний networking
- **Ingress** - внешний доступ
- **ConfigMap** - конфигурация
- **Secret** - чувствительные данные

#### Production Deployment

1. **Подготовка secrets:**
   ```bash
   kubectl create secret generic db-credentials \
     --from-literal=connection-string='...' \
     -n freaks-prod
   ```

2. **Deploy через Werf:**
   ```bash
   werf converge --env production
   ```

3. **Мониторинг:**
   ```bash
   # Проверить статус подов
   kubectl get pods -n freaks-prod

   # Просмотреть логи
   kubectl logs -f deployment/portal-webapi -n freaks-prod
   ```

#### Rollback

```bash
# Откатить deployment на предыдущую версию
kubectl rollout undo deployment/portal-webapi -n freaks-prod

# Или через Werf
werf dismiss --env production
```

---

### Docker Production Build (альтернатива)

Если не используете Kubernetes, можно развернуть через Docker Compose:

```bash
# Собрать production образы
docker-compose -f docker/docker-compose-prod.yml build

# Запустить все сервисы
docker-compose -f docker/docker-compose-prod.yml up -d

# Проверить статус
docker-compose -f docker/docker-compose-prod.yml ps
```

---

## 🔧 Troubleshooting

### Контейнеры не запускаются

```bash
# Проверить логи всех сервисов
docker-compose logs

# Проверить конкретный сервис
docker-compose logs portal-api

# Пересоздать контейнеры с нуля
docker-compose down -v
docker-compose up -d --build
```

### Ошибки миграций EF Core

```bash
# Откатить все миграции
dotnet ef database update 0 \
  --project Services/Portal/Dal/Freaks.Portal.Dal.Persistence \
  --startup-project Services/Portal/Freaks.Portal.WebApi

# Удалить базу данных и применить заново
dotnet ef database drop --force
dotnet ef database update
```

### Frontend не подключается к Backend API

**Проверки:**
1. Убедитесь что Backend API запущен: http://localhost:5100/swagger
2. Проверьте переменные окружения в `.env.local`
3. Проверьте CORS настройки в Backend (`appsettings.json`)
4. Откройте DevTools → Network и проверьте статус запросов

### Проблемы с Keycloak аутентификацией

1. Проверьте что Keycloak доступен: https://auth.freaks-aa.ru/
2. Убедитесь что клиент настроен в Keycloak realm
3. Проверьте `NEXTAUTH_SECRET` в `.env.local`
4. Очистите cookies браузера и попробуйте снова

---

## 💡 Полезные команды

### Docker

```bash
# Просмотр логов в реальном времени
docker-compose logs -f

# Остановить все сервисы
docker-compose down

# Очистить volumes (ВНИМАНИЕ: удалит данные БД)
docker-compose down -v

# Пересобрать образы без кеша
docker-compose build --no-cache

# Выполнить команду внутри контейнера
docker-compose exec portal-api bash
docker-compose exec postgres psql -U freaks -d freaks

# Проверить статус сервисов
docker-compose ps

# Посмотреть использование ресурсов
docker stats
```

### .NET

```bash
# Восстановить зависимости
dotnet restore

# Собрать решение
dotnet build

# Запустить тесты
dotnet test

# Очистить артефакты сборки
dotnet clean
```

### Frontend

```bash
# Очистить node_modules и переустановить
rm -rf node_modules package-lock.json
npm install

# Проверить устаревшие зависимости
npm outdated

# Обновить зависимости
npm update

# Проверить код линтером
npm run lint

# Собрать production build
npm run build
```

---

## ⚒️ Development Notes

### Архитектурные принципы

- Все сервисы используют окружение через `.env` и `appsettings.json`
- Локальные сервисы используют порты 5100, 5110 и т.д. (Portal, Files...)
- Каждый микросервис имеет собственную API документацию через Swagger/NSwag
- Realm management и роли пользователей синхронизируются через Keycloak Admin API
- Real-time уведомления доставляются через Centrifugo (WebSocket/SSE)

### Кеширование

Проект использует **EasyCaching** с несколькими провайдерами:
- **Redis** - распределенный кеш для production
- **In-Memory** - локальный кеш для быстрого доступа
- **Hybrid Cache** - комбинированная стратегия (In-Memory + Redis)

### Сериализация

- **MessagePack** для high-performance бинарной сериализации
- **JSON** для API контрактов (System.Text.Json)

### База данных

**Schema:** `public` (по умолчанию)

**Основные таблицы:**
- `users` - пользователи системы
- `raid`, `raid_participant`, `raid_loot`, `raid_screenshot`
- `salary`, `salary_member`, `salary_loot`, `salary_expenses`
- `auction_item`, `auction_item_bid`
- `shop_item`, `shop_item_request`
- `loot_item`
- `notification_channel`, `notification_channel_message`

**Миграции:** Управляются через EF Core Migrations, применяются автоматически при запуске

### Аутентификация Flow

1. Пользователь нажимает "Войти" в Frontend
2. Next-Auth перенаправляет на Keycloak Authorization endpoint
3. Пользователь аутентифицируется в Keycloak
4. Keycloak возвращает Authorization Code
5. Next-Auth обменивает код на Access Token и Refresh Token
6. Токены сохраняются в сессии
7. Access Token используется для авторизации API запросов к Backend
8. Refresh Token автоматически обновляет сессию при истечении

### Используемые технологии (детально)

**Backend:**
- **Mapster 7.4+** - маппинг объектов (альтернатива AutoMapper)
- **EasyCaching 1.9.2** - абстракция кеширования
- **Z.EntityFramework.Plus** - расширения для EF Core
- **Hangfire** - фоновые задачи (если используется)
- **MinIO 7.0** - S3 SDK для работы с файлами

**Frontend:**
- **Framer Motion 12.23.9** - анимации и transitions
- **React Day Picker 9.9** - компонент выбора даты
- **React Dropzone 14.3.8** - загрузка файлов drag & drop
- **Clsx 2.1.1** - утилита для условных классов CSS

---

## 🤝 Contributing

Мы приветствуем вклад в проект! Если вы хотите помочь:

1. **Fork** репозитория
2. Создайте **feature branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit** изменения (`git commit -m 'Add some AmazingFeature'`)
4. **Push** в branch (`git push origin feature/AmazingFeature`)
5. Откройте **Pull Request**

### Guidelines

- Следуйте существующему code style
- Добавляйте тесты для новой функциональности
- Обновляйте документацию при необходимости
- Используйте осмысленные commit messages

---

## 🗺 Roadmap

Планы развития проекта:

- [ ] **Система достижений** для участников гильдии
- [ ] **Мобильное приложение** (React Native)
- [ ] **Интеграция с игровым API ArcheAge** (если доступно)
- [ ] **Расширенная аналитика** с графиками и отчетами
- [ ] **Система крафта и рецептов**
- [ ] **Планировщик событий** с календарем
- [ ] **Внутренняя wiki** для гильдии
- [ ] **Система репутации** участников
- [ ] **Multi-tenant поддержка** для нескольких гильдий
- [ ] **GraphQL API** как альтернатива REST

---

## 📞 Контакты и поддержка

- **GitHub Repository:** https://github.com/Xpymb/freaks_aa
- **Issues:** https://github.com/Xpymb/freaks_aa/issues
- **Discord:** [Ссылка на сервер Discord гильдии]

---

## 📄 Лицензия

Этот проект лицензирован под лицензией **MIT** - см. файл [LICENSE](LICENSE) для деталей.

**Copyright © 2025 Freaks Guild**

---

<div align="center">

Made with ❤️ by Freaks Guild

**[⬆ Вернуться к началу](#freaks-archeage-guild-crm)**

</div>
