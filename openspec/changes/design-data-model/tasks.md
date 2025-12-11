## 1. Design & Spec
- [x] 1.1 Finalize `design.md` with detailed entity definitions, relationships, and schema (including Reminder entity)
- [ ] 1.2 Complete spec deltas for `data-model` capability
- [ ] 1.3 Review with stakeholders for alignment on domain model

## 2. Implementation
- [ ] 2.1 Create EF Core entity classes in C# (UserGroup, User, UserGroupMember, MetricType, MetricEntry, Goal, Category, Task, TaskCollaborator, TaskTimer, Reminder, YieldSummary)
- [ ] 2.2 Define DbContext with relationships and configurations
- [ ] 2.3 Create initial database migration scripts
- [ ] 2.4 Add data annotations for validations and constraints
- [ ] 2.5 Implement audit fields (created_at, updated_at) and soft deletes if needed

## 3. Validation
- [ ] 3.1 Run migration on test database and verify schema
- [ ] 3.2 Write unit tests for model validations
- [ ] 3.3 Ensure data isolation per user group (row-level security or app-level checks)
- [ ] 3.4 Validate timezone handling and UTC normalization

## 4. Documentation
- [ ] 4.1 Update `openspec/specs/` with stable data-model spec
- [ ] 4.2 Document schema in README or separate docs