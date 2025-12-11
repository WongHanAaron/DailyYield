## Context
This design outlines the core data model for Daily-Yield, a user group productivity tracking app. It defines entities for user groups, users, metrics, tasks, and summaries, ensuring scalable and secure multi-tenant data storage.

## Goals
- Support multi-user user group accounts with clear ownership and membership; users can belong to multiple user groups.
- Flexible metric system for numeric (time/count/volume), boolean, or categorical types.- Goal setting for metrics with timeframes and progress tracking; goals can be individual or user group-wide, one-time or recurring (e.g., max swears/day, weight by date, daily water).- Task tracking with timers, completion, and collaboration (single owner, multiple collaborators).
- Daily aggregation for productivity summaries.
- Data isolation per user group for privacy.

## Non-Goals
- Advanced analytics or historical data beyond retention policies.
- Integration with external APIs (handled separately).
- Mobile-specific schemas (web-first).

## Decisions
- Use Postgres for relational data with EF Core for ORM.
- Normalize entities to avoid duplication (e.g., MetricType separate from MetricEntry).
- Use UUIDs for primary keys for global uniqueness.
- Store timestamps in UTC; handle timezone display in app layer.
- Implement user_group_id on key tables for data isolation.

## Risks / Trade-offs
- Normalization vs. performance: Normalized for flexibility, but may require joins; monitor query performance.
- UUIDs: Better for distributed systems but larger than integers; acceptable for this scale.
- Soft deletes: Considered but not implemented initially; can add if needed.

## Migration Plan
- Initial migration creates all tables.
- Future changes via EF migrations.
- Data seeding for initial MetricTypes if required.

## Domain Models (draft)

### UserGroup
- id: UUID (PK)
- name: string (required, unique per user?)
- timezone: string (e.g., "America/New_York", default UTC)
- settings: JSON (optional, for user group prefs)
- created_at: timestamp
- updated_at: timestamp

### User
- id: UUID (PK)
- email: string (required, unique)
- display_name: string (required)
- created_at: timestamp
- updated_at: timestamp

### UserGroupMember
- id: UUID (PK)
- user_group_id: UUID (FK to UserGroup)
- user_id: UUID (FK to User)
- role: enum (owner, member)
- joined_at: timestamp

### MetricType
- id: UUID (PK)
- user_group_id: UUID (FK to UserGroup)
- key: string (required, unique per user group, e.g., "minutes_walked")
- display_name: string (required)
- type: enum (numeric, boolean, categorical)
- unit: string (optional, for numeric, e.g., "minutes", "gallons")
- aggregation: enum (sum, avg, count) (for numeric types)
- categories: JSON (optional, array of strings for categorical types)
- created_at: timestamp

### MetricEntry
- id: UUID (PK)
- user_id: UUID (FK to User)
- metric_type_id: UUID (FK to MetricType)
- type: enum (numeric, boolean, categorical) (inherited from MetricType for validation)
- numeric_value: decimal (nullable, for numeric metrics)
- boolean_value: bool (nullable, for boolean metrics)
- category_value: string (nullable, for categorical metrics)
- started_at: timestamp (optional, for durations)
- ended_at: timestamp (optional)
- timestamp: timestamp (required, when logged)
- metadata: JSON (optional, for extra context)
- created_at: timestamp

### Goal
- id: UUID (PK)
- user_id: UUID (FK to User, nullable for user group goals)
- user_group_id: UUID (FK to UserGroup, nullable for individual goals)
- metric_type_id: UUID (FK to MetricType)
- goal_type: enum (one_time, recurring)
- comparison: enum (at_least, at_most, exactly) (used for numeric/boolean metrics; ignored for categorical)
- target_value: string (required, target as string, matching metric type)
- frequency: enum (daily, weekly, monthly) (required for recurring goals; ignored for one_time)
- timeframe_start: date (optional, start date for one_time goals or recurring periods)
- timeframe_end: date (optional, end date for one_time goals; for recurring, may indicate when to stop)
- status: enum (active, achieved, expired)
- created_at: timestamp
- updated_at: timestamp

### Category
- id: UUID (PK)
- user_group_id: UUID (FK to UserGroup)
- name: string (required, unique per user group)
- description: string (optional)
- created_at: timestamp

### Task
- id: UUID (PK)
- owner_id: UUID (FK to User, the task owner)
- category_id: UUID (FK to Category, nullable)
- title: string (required)
- status: enum (pending, in_progress, completed)
- created_at: timestamp
- updated_at: timestamp

### TaskCollaborator
- id: UUID (PK)
- task_id: UUID (FK to Task)
- user_id: UUID (FK to User)
- role: enum (owner, collaborator) (optional, default collaborator)
- added_at: timestamp

### TaskTimer
- id: UUID (PK)
- task_id: UUID (FK to Task)
- user_id: UUID (FK to User, the collaborator who started the timer)
- started_at: timestamp (required)
- stopped_at: timestamp (nullable)
- duration: int (seconds, computed)
- created_at: timestamp

### YieldSummary
- id: UUID (PK)
- user_id: UUID (FK to User)
- user_group_id: UUID (FK to UserGroup)
- date: date (YYYY-MM-DD)
- metrics_summary: JSON (aggregated values per metric type)
- tasks_summary: JSON (total time, completed tasks)
- computed_at: timestamp

### Reminder
- id: UUID (PK)
- user_id: UUID (FK to User, nullable for user group reminders)
- user_group_id: UUID (FK to UserGroup, nullable for individual reminders)
- task_id: UUID (FK to Task, nullable)
- metric_type_id: UUID (FK to MetricType, nullable)
- title: string (required)
- message: string (optional)
- reminder_type: enum (one_time, recurring)
- schedule: JSON (cron expression for recurring; date/time for one_time)
- is_active: bool (default true)
- last_triggered: timestamp (nullable)
- created_at: timestamp
- updated_at: timestamp

## Relationships
- UserGroup 1:N UserGroupMember
- UserGroupMember N:1 User
- UserGroup 1:N MetricType
- MetricType 1:N MetricEntry
- User 1:N MetricEntry
- User 1:N Goal
- UserGroup 1:N Goal
- Goal N:1 MetricType
- UserGroup 1:N Category
- Category 1:N Task
- User 1:N Task (as owner)
- Task 1:N TaskCollaborator
- TaskCollaborator N:1 User
- Task 1:N TaskTimer
- User 1:N YieldSummary
- UserGroup 1:N YieldSummary (via UserGroupMember)
- User 1:N Reminder
- UserGroup 1:N Reminder
- Task 1:N Reminder (optional)
- MetricType 1:N Reminder (optional)

## Schema Notes
- Indexes on foreign keys and timestamps for performance.
- Constraints: Ensure at least one owner per user group via UserGroupMember; unique (user_group_id, user_id) in UserGroupMember; unique (user_group_id, key) in MetricType; unique (user_group_id, name) in Category; for Goal, frequency required if goal_type=recurring, comparison only for numeric/boolean MetricTypes; for Reminder, either user_id or user_group_id must be set, and either task_id or metric_type_id must be set.
- Timezone: Store as string (IANA); app handles conversion.
- Data retention: Implement via background jobs (not in schema).

## Open Questions
- Should MetricType be global or per user group? (Per user group for customization.)
- Need for task categories or tags? (Yes, added Category entity per user group.)
- Audit logging for changes? (Add if compliance requires.)