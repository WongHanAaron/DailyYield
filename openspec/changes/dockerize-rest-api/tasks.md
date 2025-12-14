## 1. Design & Spec
- [x] 1.1 Finalize `design.md` with Docker architecture
- [x] 1.2 Complete spec deltas for `api` capability (containerization)
- [ ] 1.3 Review with stakeholders for Docker setup

## 2. Implementation
- [ ] 2.1 Create multi-stage Dockerfile for ASP.NET Core API
- [ ] 2.2 Add docker-compose.yml with API, Postgres, Redis services
- [ ] 2.3 Configure environment variables for connections and secrets
- [ ] 2.4 Add health check endpoint to the API
- [ ] 2.5 Update build scripts to support Docker builds

## 3. Validation
- [ ] 3.1 Build Docker image successfully
- [ ] 3.2 Run docker-compose and verify services start
- [ ] 3.3 Test API endpoints via container
- [ ] 3.4 Validate database and Redis connections in container
- [ ] 3.5 Check health endpoint and logs

## 4. Documentation
- [ ] 4.1 Update `openspec/specs/` with modified api spec
- [ ] 4.2 Document Docker setup and commands in README