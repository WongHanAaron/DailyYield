#!/bin/bash

# DailyYield Stack Teardown Script
# This script stops and removes the Docker services.

set -e

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SCRIPTS_DIR="$PROJECT_ROOT/scripts"

echo "DailyYield Stack Teardown"
echo "========================="
echo ""

# Navigate to scripts directory
cd "$SCRIPTS_DIR"

echo "Stopping and removing Docker services..."
docker compose down

echo ""
echo "Teardown complete!"
echo "=================="
echo "All services stopped and containers removed."
echo ""
echo "To also remove volumes (including database data), run:"
echo "  docker compose down -v"
echo "Or from the scripts directory: docker compose down -v"