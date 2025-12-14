# Change: design-data-model

## Why
To establish the core data model for the Daily-Yield user group productivity application, defining entities, relationships, and schemas that support multi-user user group accounts, metric tracking, task management, goals, categories, reminders, and reporting. This foundational design ensures consistency across auth, metrics, tasks, and reports capabilities.

## What Changes
- Define core entities: UserGroup, User, UserGroupMember, MetricType, MetricEntry, Goal, Category, Task, TaskCollaborator, TaskTimer, Reminder, YieldSummary
- Establish relationships and constraints (e.g., user group ownership, user memberships)
- Design database schema (Postgres) with tables, indexes, and foreign keys
- Include data types, validations, and audit fields (created_at, updated_at)
- Ensure timezone handling and data isolation per user group
- Support flexible metrics (numeric, boolean, categorical), goals with timeframes, task collaboration, and reminders for tasks/metrics

## Impact
- Affected specs: `data-model` (new foundational capability)
- Affected code: Database models, migrations, and initial schema setup
- Backwards compatibility: N/A (new project)