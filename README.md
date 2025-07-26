# Freaks ArcheAge Guild CRM

**Freaks AA** is a CRM system designed to manage a guild in the MMORPG **ArcheAge**.  
It’s built using a modular microservices architecture powered by **.NET 9**, **Keycloak**, **PostgreSQL**, **Redis**, **RabbitMQ**, and **MinIO**.

The platform allows guild administrators to organize raids, manage users and their roles, track activity, and integrate with external services like **Discord**.

---

## 🧠 Project Structure

src/<br>
├── backend/<br>
│ └── dotnet/<br>
│ ├── Services/<br>
│ │ ├── Portal/ # Main business logic (users, raids, guilds, etc.)<br>
│ │ ├── Files/ # File storage service<br>
│ │ └── Bots/ # Integration bots (e.g., Discord, Telegram)<br>
│ ├── Shared/ # Shared infrastructure and utilities<br>
│ └── Freaks.sln # .NET solution file<br>
├── frontend/ # Web front-end (Next.js or similar)<br>
docker/ # Docker configurations for local development<br>

---

## 🚀 Getting Started

To run the project locally using Docker Compose:

### 1. Clone the repository

```bash
git clone https://github.com/Xpymb/freaks_aa.git
cd freaks_aa
```

---

### 2. Prepare environment file

The project uses a .env file stored in the root of the docker/ folder. To initialize it:
```bash
cd docker
./setup-env.sh        # for Linux/macOS
setup-env.cmd         # for Windows
```
This script will:
- Copy .env.local to .env
- Prompt you to enter your KEYCLOAK_ADMIN_CLIENT_SECRET

---

### 3. Run services

There are convenience scripts for starting the environment:
- Run full backend:
```bash
./run_docker-compose-local-back.sh    # for Linux/macOS
run_docker-compose-local-back.cmd     # for Windows
```
- Run databases only:
```bash
./run_docker-compose-local-db.sh     # for Linux/macOS
run_docker-compose-local-db.cmd      # for Windows
```

---

### 📦 Dependencies
- .NET 9 SDK
- Docker + Docker Compose
- Node.js (for frontend development)
- Keycloak (runs via docker)
- PostgreSQL, Redis, RabbitMQ, MinIO (included in docker-compose)

---

### 🔐 Authentication

The system uses Keycloak as the identity provider. After starting the system, Keycloak will be available at:
```bash
https://auth.freaks-aa.ru/
```
The **swagger-ui** clients are preconfigured with OAuth2 support in Swagger using **Authorization Code** flow.

---

### ⚒️ Development Notes
- All service-specific environment and secrets are configured via .env
- Local services use ports like 5010, 5020 etc. (per microservice)
- Each microservice has its own API + NSwag documentation
- Realm management and user roles are synced using Keycloak Admin API

---

### 📄 License
This project is under active development and licensed under MIT.
