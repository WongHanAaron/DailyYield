## MODIFIED Requirements

### Requirement: API Containerization
The API SHALL be containerized with Docker, providing a Dockerfile and docker-compose.yml for local development and deployment.

#### Scenario: Build Docker image
- **WHEN** docker build is executed on the API project
- **THEN** creates a multi-stage image with optimized size and runtime dependencies

#### Scenario: Run with docker-compose
- **WHEN** docker-compose up is run in the project root
- **THEN** starts the API service on port 8080, connected to Postgres and Redis containers
- **AND** supports environment variable configuration for database URLs and secrets

#### Scenario: Health checks
- **WHEN** the container is running
- **THEN** health endpoint responds for monitoring