using DailyYield.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Adapter.Database;

public class DailyYieldDbContext : DbContext
{
    public DailyYieldDbContext(DbContextOptions<DailyYieldDbContext> options) : base(options) { }

    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroupMember> UserGroupMembers { get; set; }
    public DbSet<MetricType> MetricTypes { get; set; }
    public DbSet<MetricEntry> MetricEntries { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<TaskTimer> TaskTimers { get; set; }
    public DbSet<TaskCollaborator> TaskCollaborators { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Reminder> Reminders { get; set; }
    public DbSet<YieldSummary> YieldSummaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure entities
        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Timezone).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<UserGroupMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.UserGroupId }).IsUnique();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
            entity.HasOne(e => e.User).WithMany(u => u.GroupMemberships).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.UserGroup).WithMany(ug => ug.Members).HasForeignKey(e => e.UserGroupId);
        });

        modelBuilder.Entity<MetricType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.Key, e.UserGroupId }).IsUnique();
            entity.Property(e => e.Key).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).HasConversion<string>().IsRequired().HasMaxLength(20);
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.HasOne(e => e.UserGroup).WithMany().HasForeignKey(e => e.UserGroupId);
        });

        modelBuilder.Entity<MetricEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Metadata).HasMaxLength(1000);
            entity.HasOne(e => e.User).WithMany(u => u.MetricEntries).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.MetricType).WithMany(mt => mt.Entries).HasForeignKey(e => e.MetricTypeId);
        });

        modelBuilder.Entity<DailyYield.Domain.Entities.Task>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.HasOne(e => e.Owner).WithMany(u => u.Tasks).HasForeignKey(e => e.OwnerId);
        });

        modelBuilder.Entity<TaskTimer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Task).WithMany(t => t.Timers).HasForeignKey(e => e.TaskId);
        });

        modelBuilder.Entity<TaskCollaborator>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TaskId, e.UserId }).IsUnique();
            entity.HasOne(e => e.Task).WithMany(t => t.Collaborators).HasForeignKey(e => e.TaskId);
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GoalType).HasConversion<string>().IsRequired().HasMaxLength(20);
            entity.Property(e => e.Frequency).HasMaxLength(50);
            entity.Property(e => e.Comparison).HasConversion<string>().IsRequired().HasMaxLength(20);
            entity.Property(e => e.TargetValue).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.MetricType).WithMany(mt => mt.Goals).HasForeignKey(e => e.MetricTypeId);
            entity.HasOne(e => e.User).WithMany(u => u.Goals).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.UserGroup).WithMany().HasForeignKey(e => e.UserGroupId);
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.RecurrencePattern).HasMaxLength(500);
            entity.Property(e => e.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
            entity.HasOne(e => e.User).WithMany(u => u.Reminders).HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<YieldSummary>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Date }).IsUnique();
            entity.Property(e => e.SummaryData).HasColumnType("jsonb");
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        });
    }
}