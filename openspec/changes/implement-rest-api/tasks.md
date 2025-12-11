## 1. Design & Spec
- [x] 1.1 Finalize `design.md` with API architecture decisions
- [x] 1.2 Complete spec deltas for `api` capability
- [ ] 1.3 Review with stakeholders for API design alignment

## 2. Implementation
- [ ] 2.1 Set up ASP.NET Core Web API project with EF Core integration
- [ ] 2.2 Define domain ports (interfaces) for repositories and services
- [ ] 2.3 Implement adapters: EF Core repository adapters for data access
- [ ] 2.4 Implement CQRS: define command and query classes, handlers for each operation
- [ ] 2.5 Implement authentication: JWT token generation and validation
- [ ] 2.6 Create base controller classes with authorization filters
- [ ] 2.7 Implement UserGroup controllers (CRUD) as primary adapters calling commands/queries
- [ ] 2.8 Implement User controllers (register, login)
- [ ] 2.9 Implement MetricType and MetricEntry controllers
- [ ] 2.10 Implement Task, TaskTimer, TaskCollaborator controllers
- [ ] 2.11 Implement Goal controllers
- [ ] 2.12 Implement Reminder controllers
- [ ] 2.13 Implement YieldSummary read controllers
- [ ] 2.14 Add input validation, error handling, and logging
- [ ] 2.15 Configure CORS and basic rate limiting

## 3. Validation
- [ ] 3.1 Write unit tests for command and query handlers
- [ ] 3.2 Write unit tests for repository adapters and ports
- [ ] 3.3 Write unit tests for controllers and services
- [ ] 3.4 Run integration tests against test database
- [ ] 3.5 Test authentication and authorization flows
- [ ] 3.6 Validate API responses match spec scenarios
- [ ] 3.7 Ensure data isolation per user group

## 4. Documentation
- [ ] 4.1 Generate Swagger/OpenAPI docs
- [ ] 4.2 Update `openspec/specs/` with stable api spec
- [ ] 4.3 Document API endpoints in README