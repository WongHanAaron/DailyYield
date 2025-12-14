## ADDED Requirements

### Requirement: Core Data Model Entities
The system SHALL define and persist core entities: UserGroup, UserGroupMember, User, MetricType, MetricEntry, Goal, Category, Task, TaskCollaborator, TaskTimer, Reminder, and YieldSummary with specified attributes and relationships.

#### Scenario: UserGroup creation and user membership
- **WHEN** a user group is created
- **THEN** a UserGroup record is stored with id, name, timezone
- **AND** users can join multiple user groups via UserGroupMember records with roles

#### Scenario: Metric logging
- **WHEN** a user logs a numeric metric entry (e.g., minutes walked)
- **THEN** a MetricEntry is stored with numeric_value set
- **WHEN** a user logs a boolean metric (e.g., did exercise)
- **THEN** boolean_value is true or false
- **WHEN** a user logs a categorical metric (e.g., mood: happy)
- **THEN** category_value is the category name

#### Scenario: Goal setting
- **WHEN** a user sets an individual goal for a metric type (e.g., walk 10000 minutes this month)
- **THEN** a Goal record is stored with user_id set and target_value, timeframe
- **WHEN** a user group sets a shared goal (e.g., group walks 50000 minutes this month)
- **THEN** a Goal record is stored with user_group_id set
- **WHEN** setting a recurring goal (e.g., drink water daily, max 1 swear/day)
- **THEN** goal_type is recurring with frequency and comparison

#### Scenario: Task and timer tracking
- **WHEN** a user starts a task timer
- **THEN** a TaskTimer record is created with started_at
- **WHEN** stopped, duration is computed and stored
- **AND** collaborators can be added to a task via TaskCollaborator records

#### Scenario: Reminder creation
- **WHEN** a user creates a reminder for a task
- **THEN** a Reminder record is stored with user_id, task_id, and schedule
- **WHEN** a user group creates a reminder for a metric
- **THEN** a Reminder record is stored with user_group_id, metric_type_id, and schedule

#### Scenario: Daily summary computation
- **WHEN** aggregation runs for a date
- **THEN** YieldSummary records are created per user with aggregated metrics and tasks