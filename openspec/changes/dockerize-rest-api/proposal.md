# Change: dockerize-rest-api

## Why
To facilitate deployment and local development of the Daily-Yield REST API, we need to containerize it with Docker. This allows for consistent environments, easy scaling, and integration with Docker Compose for full-stack testing including the database and cache services.

## What Changes
- Create a multi-stage Dockerfile for the ASP.NET Core API to optimize image size
- Add docker-compose.yml to orchestrate the API, PostgreSQL, and Redis services
- Configure environment variables for database connections and JWT secrets
- Ensure the container exposes the correct port and handles health checks
- Update build and run scripts to support Docker execution

## Impact
- Affected specs: `api` (modified capability with containerization)
- Affected code: Adds Dockerfile and docker-compose.yml to the API project
- Backwards compatibility: Maintains existing functionality, adds Docker as an option