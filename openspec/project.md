# Project Context

## Purpose
Daily-Yield is a full-stack web application designed to help families track and improve their daily productivity and healthy habits. Multiple users within a family can collaborate to monitor time spent on tasks, track task completions, and log personal metrics such as minutes walked, gallons of water consumed, or other custom health/productivity indicators. The app provides daily summaries, dashboards, and optional exports to encourage accountability and progress toward personal goals.

## Tech Stack
- Backend: .NET (e.g., .NET 8) + C#
- API: RESTful endpoints (ASP.NET Core)
- Scheduler / Workers: Hangfire (Redis) or Quartz.NET for background jobs
- Database: PostgreSQL (primary), Redis (cache, job broker)
- Frontend: React + Vite (SPA with responsive design for mobile/web)
- Infrastructure: Docker + Docker Compose for local development
- CI/CD: GitHub Actions
- Observability: Prometheus metrics + Grafana (or simple logging to CloudWatch/Datadog)

## Project Conventions

### Project Structure
- Components are organized as `<component>/<component_solution_files>`
- For C# projects, follow the structure: `<component>/<solution_file>`, `<component>/src/<source_projects>`, and `<component>/tests/<test_projects>`

### Code Style
- C# for backend code; use StyleCop or EditorConfig with project config
- Use nullable reference types and strict mode in C#
- Naming: `camelCase` for variables, `PascalCase` for types/classes, kebab-case for file/folder names
- Follow Conventional Commits for commit messages (`feat:`, `fix:`, `chore:`, `docs:`, `refactor:`)

### Architecture Patterns
- Modular monolith split into clear domains: `auth`, `metrics`, `tasks`, `reports`
- Single service owns the productivity domain; background workers perform scheduled aggregation and export tasks
- Clear separation of read/write models (Postgres primary store; Redis for ephemeral state)
- Keep APIs small and well-documented; prefer backward-compatible additive changes

### Testing Strategy
- Unit tests: xUnit (or NUnit) for business logic and utilities
- Integration tests: run against ephemeral Postgres (via `docker-compose`) and Redis for worker flows
- End-to-end smoke tests: basic flow for metric logging → task tracking → report generation (run in CI nightly or on PRs for major changes)
- Require tests for any new backend logic that affects calculations or persistence

#### Unit Test Rules
- Do not declare re-used dependencies in the test class
- Declare all member variables in the test method
- Call the component under test as 'sut' (System Under Test)

### Git Workflow
- Use GitHub Flow: feature branches off `main`, open PRs for review
- Branch naming: `proposal/<short-desc>` `feat/<short-desc>`, `fix/<short-desc>`, `chore/<short-desc>`
- Use Draft PRs for WIP; require at least one approval for merging to `main`
- CI must pass (lint, tests) before merging; use squash merges and ensure meaningful commit messages

## Domain Context
- Core entities: `Family`, `User`, `MetricType`, `MetricEntry`, `Task`, `TaskTimer`, `YieldSummary` (daily productivity aggregation)
- Ingestion: users log metric entries (e.g., time walked) and track task timers; system aggregates daily summaries per user and family
- Aggregation rules: daily summaries aligned to family timezone (UTC default); include totals for metrics and task durations
- Reporting: families can view dashboards, subscribe to daily email summaries, and download CSV exports

## Important Constraints
- Accuracy: calculations must be deterministic and auditable (store raw inputs and computed deltas)
- Timezones: normalize timestamps to UTC and keep family timezone metadata for aggregation windows
- Data retention: raw `MetricEntry` and `TaskTimer` retained for at least 1 year; summaries retained longer as configured
- Security & compliance: treat family data as sensitive; protect PII, require encrypted transports and at-rest encryption for production databases; ensure family-level data isolation

## External Dependencies
- Postgres (managed or self-hosted)
- Redis (job queue/cache)
- SMTP provider (SendGrid, SES) for scheduled email reports
- Object storage (S3 or S3-compatible) for exports (CSV/Parquet)
- Optional: metrics/logging provider (Datadog/CloudWatch)

## Contacts and Roles
- Maintainers: (add names/emails here)
- On-call: (add on-call rotation or escalation docs)

## Notes for AI Assistants
- Before proposing a change, read `openspec/AGENTS.md` and existing specs in `openspec/specs/`.
- Prefer small, non-breaking changes and create formal OpenSpec proposals for new features or breaking changes.
