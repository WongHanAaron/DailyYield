#!/bin/bash

# Environment Setup Script for DailyYield REST API
# This script creates a default .env file for local development.

set -e

ENV_FILE="scripts/.env"
EXAMPLE_FILE="scripts/.env.example"

echo "DailyYield REST API Environment Setup"
echo "====================================="
echo "Creating default .env file for local development..."
echo ""

# Check if .env already exists
if [ -f "$ENV_FILE" ]; then
    read -p ".env file already exists. Overwrite with defaults? (y/N): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        echo "Setup cancelled."
        exit 0
    fi
fi

# Set default values for local development
ASPNETCORE_ENVIRONMENT="Development"
ASPNETCORE_URLS="http://+:8080"
POSTGRES_DB="dailyyield"
POSTGRES_USER="postgres"
POSTGRES_PASSWORD="password"

# Generate JWT Secret Key
JWT_SECRET_KEY=$(openssl rand -hex 32)
echo "Generated JWT Secret Key: ${JWT_SECRET_KEY:0:10}..."

JWT_ISSUER="DailyYield"
JWT_AUDIENCE="DailyYieldUsers"

# Create .env file
cat > "$ENV_FILE" << EOF
# Environment variables for DailyYield REST API (Local Development)
ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ASPNETCORE_URLS=$ASPNETCORE_URLS
POSTGRES_DB=$POSTGRES_DB
POSTGRES_USER=$POSTGRES_USER
POSTGRES_PASSWORD=$POSTGRES_PASSWORD
JWT_SECRET_KEY=$JWT_SECRET_KEY
JWT_ISSUER=$JWT_ISSUER
JWT_AUDIENCE=$JWT_AUDIENCE
EOF

echo ".env file created successfully with default values!"

# Create .env.example if it doesn't exist
if [ ! -f "$EXAMPLE_FILE" ]; then
    cat > "$EXAMPLE_FILE" << EOF
# Example environment variables for DailyYield REST API
# Copy this file to .env and fill in the values
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:8080
POSTGRES_DB=dailyyield
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_password_here
JWT_SECRET_KEY=your_secret_key_here
JWT_ISSUER=DailyYield
JWT_AUDIENCE=DailyYieldUsers
EOF
    echo ".env.example file created for reference."
fi

echo ""
echo "Setup complete! You can now navigate to rest-api and run 'docker-compose up --build' to start the services."
echo "Default PostgreSQL password is 'password' - change it for production!"
echo "Don't forget to run database migrations: 'docker-compose exec api dotnet ef database update'"