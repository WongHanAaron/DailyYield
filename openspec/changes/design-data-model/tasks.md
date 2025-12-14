## 1. Design & Spec
- [x] 1.1 Finalize `design.md` with detailed entity definitions, relationships, and schema (including Reminder entity)
- [ ] 1.2 Complete spec deltas for `data-model` capability
- [ ] 1.3 Review with stakeholders for alignment on domain model

## 2. Implementation
- [x] 2.1 Create EF Core entity classes in C# (UserGroup, User, UserGroupMember, MetricType, MetricEntry, Goal, Category, Task, TaskCollaborator, TaskTimer, Reminder, YieldSummary)
- [x] 2.2 Define DbContext with relationships and configurations
- [x] 2.3 Create initial database migration scripts
- [x] 2.4 Add data annotations for validations and constraints
- [x] 2.5 Implement audit fields (created_at, updated_at) and soft deletes if needed

## 3. Validation
- [x] 3.1 Run migration on test database and verify schema
- [x] 3.2 Write unit tests for model validations
- [x] 3.3 Ensure data isolation per user group (row-level security or app-level checks)
- [x] 3.4 Validate timezone handling and UTC normalization

## 4. Documentation
- [x] 4.1 Update `openspec/specs/` with stable data-model spec
- [x] 4.2 Document schema in README or separate docs