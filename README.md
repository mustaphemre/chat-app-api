# ChatApp

**ChatApp** is a microservice-style .NET 9 solution implementing chat and user APIs, background processing workers, and database migration utilities.  
It leverages **.NET Aspire** for service composition and hosting, **Kafka** for asynchronous messaging, **SQL Server** for data persistence, and **OpenTelemetry** for observability.

---

## 🧩 Architecture Overview

ChatApp follows a modular design with distinct projects for each bounded context:

```
src/
├── ChatApp.AppHost/                   # Aspire host – entry point & service composition
├── ChatApp.ServiceDefaults/           # Shared defaults (health checks, OpenTelemetry, resilience)
│
├── ChatApp.Chats/
│   ├── ChatApp.Chats.Api/             # Chats HTTP API
│   ├── ChatApp.Chats.Worker/          # Background worker (Kafka consumers, retry logic)
│   ├── ChatApp.Chats.Infrastructure/  # EF Core DbContext, migrations, persistence
│   └── ChatApp.Chats.Application/     # Producers, consumers, and messaging logic
│
├── ChatApp.Users/
│   └── ChatApp.Users.Api/             # Users API (exposed via HTTP and gRPC)
│
└── ChatApp.Worker.DbMigration/        # Applies EF Core migrations on startup
```

---

## 🚀 Features

- **Microservice Architecture** using .NET Aspire.
- **gRPC communication** between APIs (e.g., Chats → Users).
- **Kafka Integration** for publishing and consuming events.
- **EF Core + SQL Server** for data persistence.
- **OpenTelemetry** for distributed tracing and metrics.
- **Resilience** via retry and fallback policies in background workers.
- **Health Checks** and service discovery via Aspire defaults.

---

## ⚙️ Components

### **AppHost**
The central composition root using Aspire.  
It configures and orchestrates all microservices, including API endpoints, background workers, message brokers, and observability components.

### **Chats Service**
Handles chat creation, message persistence, and event publishing.  
- Exposes a REST API (`ChatApp.Chats.Api`)
- Consumes messages via a background worker (`ChatApp.Chats.Worker`)
- Uses SQL Server for persistence (via `ChatApp.Chats.Infrastructure`)
- Communicates with `Users.Api` via gRPC for user data

### **Users Service**
Manages user profiles and availability data.  
- Provides REST and gRPC endpoints  
- Used by the `Chats` service for user lookups

### **DbMigration Worker**
Executes EF Core migrations automatically at startup to ensure schema consistency across environments.

### **ServiceDefaults**
Provides cross-cutting configuration:
- Health checks
- OpenTelemetry setup
- Resilience policies (Polly)
- Structured logging

---

## 📦 Technologies

| Category | Technology |
|-----------|-------------|
| Framework | .NET 9 / Aspire |
| Messaging | Apache Kafka |
| Database | Microsoft SQL Server |
| ORM | Entity Framework Core |
| Communication | REST + gRPC |
| Observability | OpenTelemetry, HealthChecks |
| Resilience | Polly, Aspire ServiceDefaults |

---

## 🧰 Getting Started

### **Prerequisites**
- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/) (for Kafka and SQL Server)
- [Aspire Dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard)

### **Running the Project**

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/ChatApp.git
   cd ChatApp
   ```

2. Start the environment:
   ```bash
   docker-compose up -d
   ```

3. Run the Aspire AppHost:
   ```bash
   dotnet run --project src/ChatApp.AppHost
   ```

4. Access the Aspire dashboard (default):
   ```
   http://localhost:18888
   ```

5. API endpoints:
   - Chats API → `http://localhost:5001`
   - Users API → `http://localhost:5002`

---

## 📡 Messaging Setup

ChatApp uses **Kafka** for inter-service communication.

- **Producers:** Located in `ChatApp.Chats.Application`
- **Consumers:** Implemented in `ChatApp.Chats.Worker`
- Topics are automatically created (if enabled in configuration)

Example configuration (AppHost):

```json
{
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "TopicNames": {
      "ChatCreated": "chat.created"
    }
  }
}
```

---

## 🧱 Database & Migrations

- EF Core migrations are located under:
  ```
  src/ChatApp.Chats/ChatApp.Chats.Infrastructure/Migrations/
  ```
- The `ChatApp.Worker.DbMigration` project ensures migrations are applied on startup.
- The database provider is **SQL Server**.

---

## 🧭 Observability

OpenTelemetry is integrated via `ChatApp.ServiceDefaults`:
- Traces are exported to the Aspire dashboard.
- Metrics and logs provide real-time visibility into service behavior.

---

## 📘 License

This project is licensed under the [MIT License](LICENSE).

---

> Built with ❤️ using .NET Aspire, Kafka, and OpenTelemetry.
