# Change: implement-rest-api

## Why
To enable the front-end React application to interact with the database, we need a RESTful API built with ASP.NET Core that provides secure, authenticated endpoints for CRUD operations on core entities (UserGroups, Users, Metrics, Tasks, etc.). This API will serve as the backend service, handling data persistence, business logic, and integration with PostgreSQL via EF Core.

## What Changes
- Implement ASP.NET Core Web API project with controllers for each domain (auth, metrics, tasks, reports)
- Add JWT-based authentication and authorization with user group isolation
- Define REST endpoints for creating, reading, updating, deleting entities
- Integrate EF Core for database access with the designed data model
- Add input validation, error handling, and logging
- Ensure API responses are JSON-formatted and follow REST conventions
- Include basic rate limiting and CORS for front-end integration

## Impact
- Affected specs: `api` (new capability for backend services)
- Affected code: New API project, controllers, services, and integration with existing data model
- Backwards compatibility: N/A (new feature)