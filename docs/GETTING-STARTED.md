# Getting Started with DailyYield REST API

This guide will help you set up and run the DailyYield REST API using Docker Compose for a fully functional development stack.

## Prerequisites

- Docker and Docker Compose installed
- Bash shell (Linux/Mac) or WSL (Windows)
- OpenSSL (for JWT key generation, usually pre-installed)

## Quick Setup

### Option 1: Manual Setup
1. **Clone the repository** (if not already done):
   ```bash
   git clone <repository-url>
   cd <repository-directory>
   ```

2. **Run the setup script**:
   ```bash
   ./scripts/setup.sh
   ```
   This will create a default `.env` file in the `rest-api` directory with development-friendly values (including a generated JWT secret).

3. **Navigate to the scripts directory**:
   ```bash
   cd scripts
   ```

4. **Start the Docker services**:
   ```bash
   docker compose up --build
   ```
   This will build and start the API, PostgreSQL database, and Redis services.

5. **Run database migrations** (in a new terminal):
   ```bash
   docker compose exec api dotnet ef database update
   ```

### Option 2: Automated Setup (Recommended)
For a fully automated startup:
```bash
./scripts/start-stack.sh
```
This script will handle environment setup, start all services, wait for them to be healthy, and run database migrations automatically.

5. **Verify the setup**:
   - API: http://localhost:8080
   - Swagger UI: http://localhost:8080/swagger
   - Health check: http://localhost:8080/health

### What the Scripts Do

- **`scripts/setup.sh`**: Creates a default `.env` file for local development.
- **`scripts/start-stack.sh`**: Fully automated script that sets up environment (if needed), starts Docker services, waits for health, and runs migrations.- **`scripts/teardown.sh`**: Stops and removes all Docker services and containers.
## Environment Variables

The setup script creates a `.env` file with default values for local development:
- `ASPNETCORE_ENVIRONMENT`: Development
- `ASPNETCORE_URLS`: http://+:8080
- `POSTGRES_DB`: dailyyield
- `POSTGRES_USER`: postgres
- `POSTGRES_PASSWORD`: password (change for production!)
- `JWT_SECRET_KEY`: Auto-generated secure key
- `JWT_ISSUER`: DailyYield
- `JWT_AUDIENCE`: DailyYieldUsers

**Security Note**: The default PostgreSQL password is 'password'. Update it in `.env` for any production or shared environment.

## Troubleshooting

- **Permission denied on script**: Run `chmod +x scripts/setup.sh`
- **Docker build fails**: Ensure Docker is running and you have sufficient resources
- **Database connection issues**: Check the `.env` file values and ensure ports 5432/6379 are free
- **Migrations fail**: Ensure the API container is running before executing migrations

## Next Steps

- Explore the API via Swagger UI
- Check the main [README.md](README.md) for more details on the API features
- Run tests: `docker-compose exec api dotnet test`

## Stopping the Services

To stop the stack:
```bash
docker-compose down
```

To stop and remove volumes (including database data):
```bash
docker-compose down -v
```