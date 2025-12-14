#!/bin/bash

# DailyYield Stack Startup Script
# This script sets up the environment and starts the full Docker stack.

set -e

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SCRIPTS_DIR="$PROJECT_ROOT/scripts"

echo "DailyYield Stack Startup"
echo "========================"
echo ""

# Check if .env exists, if not, run setup
if [ ! -f "$SCRIPTS_DIR/.env" ]; then
    echo "Environment file not found. Running setup..."
    "$PROJECT_ROOT/scripts/setup.sh"
    echo ""
fi

# Navigate to scripts directory
cd "$SCRIPTS_DIR"

echo "Starting Docker services..."
docker compose up --build -d

echo "Waiting for services to be healthy..."
# Wait for API to be healthy (healthcheck is defined in docker-compose.yml)
echo "Checking API health..."
until curl -f http://localhost:8080/health &>/dev/null; do
    echo "Waiting for API to be healthy..."
    sleep 5
done

echo "API is healthy! Running database migrations..."
# Check if dotnet ef is available
if command -v dotnet >/dev/null 2>&1 && dotnet ef --version >/dev/null 2>&1; then
    # Source the .env file to set environment variables for EF
    set -a
    source .env
    set +a
    dotnet ef database update --project ../rest-api/src/DailyYield.Api --startup-project ../rest-api/src/DailyYield.Api
else
    echo "Warning: dotnet ef tool not found. Please run database migrations manually:"
    echo "  cd scripts"
    echo "  dotnet ef database update --project ../rest-api/src/DailyYield.Api --startup-project ../rest-api/src/DailyYield.Api"
fi

echo ""
echo "Stack startup complete!"
echo "========================"
echo "API: http://localhost:8080"
echo "Swagger: http://localhost:8080/swagger"
echo "Health: http://localhost:8080/health"
echo ""
echo "To view logs: docker compose logs -f"
echo "To stop: docker compose down"
echo "To teardown completely: ../scripts/teardown.sh"