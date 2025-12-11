## Context
This design outlines the implementation of a REST API for Daily-Yield, providing endpoints for the front-end to perform database operations. The API will be built with ASP.NET Core, leveraging the existing data model designed in the data-model capability.

## Goals
- Provide RESTful endpoints for all core entities (UserGroup, User, MetricType, MetricEntry, Task, etc.)
- Implement authentication and authorization to ensure users can only access their user group's data
- Support CRUD operations with proper validation and error handling
- Enable real-time or scheduled operations like metric logging, task timers, and summary generation
- Ensure scalability and security for family-based multi-tenancy

## Non-Goals
- GraphQL or other API styles (REST only)
- Advanced caching or CDN integration (basic Redis for sessions if needed)
- Mobile app direct API access (front-end proxy)

## Decisions
- Use ASP.NET Core 8 with minimal API or controllers for endpoints
- JWT tokens for authentication, stored in headers
- EF Core for ORM with the Postgres database
- AutoMapper for DTO mappings
- Serilog for logging
- Swagger/OpenAPI for documentation
- User group ID in claims for data isolation
- Implement CQRS pattern: separate command handlers for write operations (e.g., CreateUserGroup, LogMetric) and query handlers for read operations (e.g., GetUserGroups, GetDailySummary)
- Use Ports and Adapters (Hexagonal Architecture): define ports (interfaces) for domain services and repositories; implement adapters for external concerns like database (EF Core), messaging, and API controllers as primary adapters

## API Endpoints Overview
High-level list of REST API endpoints to be implemented:

- **POST /api/auth/login** - Authenticate user with email/password, return JWT token
- **POST /api/auth/register** - Register new user account
- **GET /api/usergroups** - Retrieve list of user groups for authenticated user
- **POST /api/usergroups** - Create a new user group
- **GET /api/usergroups/{id}** - Get details of a specific user group
- **PUT /api/usergroups/{id}** - Update user group details
- **DELETE /api/usergroups/{id}** - Delete a user group (if authorized)
- **POST /api/usergroups/{id}/members** - Add a member to user group
- **GET /api/metric-types** - List metric types for user's user groups
- **POST /api/metric-types** - Create a new metric type
- **GET /api/metric-types/{id}** - Get metric type details
- **PUT /api/metric-types/{id}** - Update metric type
- **DELETE /api/metric-types/{id}** - Delete metric type
- **GET /api/metric-entries** - List metric entries for user
- **POST /api/metric-entries** - Log a new metric entry
- **GET /api/metric-entries/{id}** - Get metric entry details
- **PUT /api/metric-entries/{id}** - Update metric entry
- **DELETE /api/metric-entries/{id}** - Delete metric entry
- **GET /api/tasks** - List tasks for user's user groups
- **POST /api/tasks** - Create a new task
- **GET /api/tasks/{id}** - Get task details
- **PUT /api/tasks/{id}** - Update task
- **DELETE /api/tasks/{id}** - Delete task
- **POST /api/tasks/{id}/start-timer** - Start a timer for a task
- **POST /api/tasks/{id}/stop-timer** - Stop the active timer for a task
- **GET /api/tasks/{id}/collaborators** - List collaborators for a task
- **POST /api/tasks/{id}/collaborators** - Add a collaborator to a task
- **DELETE /api/tasks/{id}/collaborators/{userId}** - Remove collaborator
- **GET /api/goals** - List goals for user/user groups
- **POST /api/goals** - Create a new goal
- **GET /api/goals/{id}** - Get goal details
- **PUT /api/goals/{id}** - Update goal
- **DELETE /api/goals/{id}** - Delete goal
- **GET /api/reminders** - List reminders for user/user groups
- **POST /api/reminders** - Create a new reminder
- **GET /api/reminders/{id}** - Get reminder details
- **PUT /api/reminders/{id}** - Update reminder
- **DELETE /api/reminders/{id}** - Delete reminder
- **GET /api/summaries** - Get daily/weekly summaries for user groups (with date filters)

All endpoints will require JWT authentication and enforce user group-based access control.

## Risks / Trade-offs
- Performance: EF Core queries may need optimization for complex aggregations; monitor with profiling
- Security: JWT expiration and refresh tokens; implement proper CORS
- Complexity: Start with basic CRUD, add business logic incrementally

## Migration Plan
- New API project alongside existing code
- Deploy as separate service or monolith initially
- Front-end updates to consume API endpoints