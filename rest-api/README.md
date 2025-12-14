# DailyYield REST API

A comprehensive REST API for the DailyYield productivity tracking application, built with ASP.NET Core, EF Core, and PostgreSQL.

## Features

- JWT-based authentication and authorization
- User group isolation for data security
- CRUD operations for metrics, tasks, goals, and reminders
- Real-time productivity summaries and analytics
- Clean Architecture with CQRS pattern

## Getting Started

### Prerequisites

- .NET 10.0 or later
- PostgreSQL database
- Visual Studio 2022 or VS Code
- Docker and Docker Compose (for containerized deployment)

### Installation

#### Option 1: Local Development

1. Clone the repository
2. Navigate to the `rest-api` directory
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Update the connection string in `appsettings.json`
5. Run database migrations:
   ```bash
   dotnet ef database update
   ```
6. Run the application:
   ```bash
   dotnet run
   ```

#### Option 2: Docker Development

1. Clone the repository
2. Navigate to the `rest-api` directory
3. Copy and configure environment variables:
   ```bash
   cp .env.example .env
   # Edit .env with your configuration
   ```
4. Start the services:
   ```bash
   docker-compose up --build
   ```
5. Run database migrations in the container:
   ```bash
   docker-compose exec api dotnet ef database update
   ```

### Docker Commands

- **Build and start services:**
  ```bash
  docker-compose up --build
  ```

- **Run in background:**
  ```bash
  docker-compose up -d --build
  ```

- **Stop services:**
  ```bash
  docker-compose down
  ```

- **View logs:**
  ```bash
  docker-compose logs -f api
  ```

- **Run database migrations:**
  ```bash
  docker-compose exec api dotnet ef database update
  ```

- **Run tests:**
  ```bash
  docker-compose exec api dotnet test
  ```

### API Documentation

The API documentation is available via Swagger UI at `http://localhost:8080/swagger` when running locally or in Docker.

## Authentication

The API uses JWT (JSON Web Tokens) for authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

### Endpoints

#### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and receive JWT token

#### User Groups
- `GET /api/usergroups` - Get user's user groups
- `POST /api/usergroups` - Create a new user group

#### Metrics
- `GET /api/metric-types` - Get metric types
- `POST /api/metric-types` - Create metric type
- `GET /api/metric-entries` - Get metric entries
- `POST /api/metric-entries` - Log metric entry

#### Tasks
- `GET /api/tasks` - Get tasks
- `POST /api/tasks` - Create task
- `POST /api/tasks/{id}/start-timer` - Start task timer
- `POST /api/tasks/{id}/stop-timer` - Stop task timer

#### Goals
- `GET /api/goals` - Get goals
- `POST /api/goals` - Create goal

#### Reminders
- `GET /api/reminders` - Get reminders
- `POST /api/reminders` - Create reminder

#### Summaries
- `GET /api/summaries` - Get productivity summaries

## Architecture

The API follows Clean Architecture principles:

- **Domain**: Core business entities and interfaces
- **Application**: CQRS commands, queries, and handlers
- **Infrastructure**: Database adapters and external services
- **API**: REST controllers and middleware

## Testing

Run the test suite:

```bash
dotnet test
```

## Contributing

1. Follow the OpenSpec workflow for proposing changes
2. Ensure all tests pass
3. Update API documentation as needed