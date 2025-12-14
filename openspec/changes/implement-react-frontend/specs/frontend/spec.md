## ADDED Requirements

### Requirement: Authentication UI
The frontend SHALL provide login and register forms that interact with the API.

#### Scenario: User login
- **WHEN** user enters email and password
- **THEN** API call to /api/auth/login, store JWT token, redirect to dashboard

#### Scenario: User registration
- **WHEN** user submits registration form
- **THEN** API call to /api/auth/register, handle success/error

### Requirement: Dashboard View
The frontend SHALL display a dashboard with user groups, recent metrics, tasks, and summaries.

#### Scenario: Load dashboard
- **WHEN** authenticated user accesses dashboard
- **THEN** fetch and display user groups, metrics, tasks, goals

### Requirement: Metric Logging UI
The frontend SHALL allow users to log metrics via forms.

#### Scenario: Log numeric metric
- **WHEN** user selects metric type and enters value
- **THEN** API call to POST /api/metric-entries

### Requirement: Task Management UI
The frontend SHALL provide CRUD for tasks and timer controls.

#### Scenario: Start task timer
- **WHEN** user clicks start on a task
- **THEN** API call to POST /api/tasks/{id}/start-timer

### Requirement: Goal Setting UI
The frontend SHALL allow setting and viewing goals.

#### Scenario: Create goal
- **WHEN** user fills goal form
- **THEN** API call to POST /api/goals

### Requirement: Summary Reports
The frontend SHALL display aggregated summaries.

#### Scenario: View daily summary
- **WHEN** user selects date
- **THEN** API call to GET /api/summaries, display charts/tables