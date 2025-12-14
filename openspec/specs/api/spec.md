## ADDED Requirements

### Requirement: Authentication and Authorization
The API SHALL support JWT-based authentication for users, with endpoints for login/register, and authorize access based on user group membership.

#### Scenario: User login
- **WHEN** a user provides valid email and password
- **THEN** the API returns a JWT token with user_id and user_group_ids in claims
- **AND** subsequent requests include the token in Authorization header

#### Scenario: Access control
- **WHEN** a request is made to access data
- **THEN** the API verifies the user belongs to the relevant user group
- **AND** denies access if not authorized

### Requirement: User Group Management Endpoints
The API SHALL provide REST endpoints for managing user groups: GET /api/usergroups, POST /api/usergroups, etc.

#### Scenario: Create user group
- **WHEN** POST /api/usergroups with name and timezone
- **THEN** a new UserGroup is created and returned
- **AND** the creator is set as owner in UserGroupMember

#### Scenario: List user groups for user
- **WHEN** GET /api/usergroups
- **THEN** returns array of user groups the authenticated user belongs to

### Requirement: Metric Endpoints
The API SHALL provide endpoints for MetricType and MetricEntry CRUD.

#### Scenario: Create metric type
- **WHEN** POST /api/metric-types with key, display_name, type, etc.
- **THEN** a MetricType is created for the user's user group

#### Scenario: Log metric entry
- **WHEN** POST /api/metric-entries with metric_type_id, value, timestamp
- **THEN** a MetricEntry is stored with user_id and validated against MetricType

### Requirement: Task Endpoints
The API SHALL provide endpoints for Task, TaskTimer, and TaskCollaborator.

#### Scenario: Start task timer
- **WHEN** POST /api/tasks/{id}/start-timer
- **THEN** a TaskTimer is created with started_at

#### Scenario: Stop task timer
- **WHEN** POST /api/tasks/{id}/stop-timer
- **THEN** the TaskTimer is updated with ended_at and duration computed

### Requirement: Goal Endpoints
The API SHALL provide CRUD for Goals.

#### Scenario: Set goal
- **WHEN** POST /api/goals with metric_type_id, target_value, etc.
- **THEN** a Goal is created for the user or user group

### Requirement: Reminder Endpoints
The API SHALL provide endpoints for Reminders.

#### Scenario: Create reminder
- **WHEN** POST /api/reminders with task_id or metric_type_id, schedule
- **THEN** a Reminder is stored

### Requirement: Summary Endpoints
The API SHALL provide read-only endpoints for YieldSummary.

#### Scenario: Get daily summary
- **WHEN** GET /api/summaries?date=2023-10-01
- **THEN** returns aggregated data for the user's user group