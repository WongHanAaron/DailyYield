## Context
This design outlines the containerization of the Daily-Yield REST API using Docker, building on the existing `implement-rest-api` change. It ensures the API can be run in isolated, reproducible environments.

## Goals
- Provide a Docker image for the API that can be deployed anywhere
- Enable local development with docker-compose including all dependencies
- Optimize image size with multi-stage builds
- Support environment-based configuration for different deployments

## Non-Goals
- Kubernetes or advanced orchestration (basic Docker Compose)
- Windows containers (Linux-based)

## Decisions
- Use multi-stage Dockerfile: build stage for compilation, runtime stage with ASP.NET Core runtime
- Docker Compose for local dev: services for API, Postgres, Redis
- Expose port 8080 for the API
- Use environment variables for connection strings and secrets
- Include health checks in the Dockerfile

## Risks / Trade-offs
- Image size: Multi-stage reduces size but may increase build time
- Local dev complexity: Docker adds overhead but ensures consistency
- Security: Ensure secrets are not baked into images

## Migration Plan
- Add Docker files to the API project
- Update CI/CD to build and push Docker images
- Document Docker usage in README