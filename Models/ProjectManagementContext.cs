using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement;

public partial class ProjectManagementContext : DbContext
{
    public ProjectManagementContext()
    {
    }

    public ProjectManagementContext(DbContextOptions<ProjectManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Phase> Phases { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(ConfigurationManager.AppSettings["ConnectionString"]);
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("department_pkey");

            entity.ToTable("department");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Blocked).HasColumnName("blocked");
            entity.Property(e => e.DateBirth).HasColumnName("dateBirth");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.New)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("new");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasColumnType("character varying")
                .HasColumnName("patronymic");
            entity.Property(e => e.Surname)
                .HasColumnType("character varying")
                .HasColumnName("surname");
            entity.Property(e => e.Admin)
                .HasColumnName("admin");
            entity.Property(e => e.Post)
                .HasColumnName("post");


            entity.HasOne(e => e.PostNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.Post)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_post_fkey");

            entity.HasMany(d => d.Projects).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectEmployee",
                    r => r.HasOne<Project>().WithMany()
                        .HasForeignKey("Project")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("projectEmployee_project_fkey"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("Employee")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("projectEmployee_employee_fkey"),
                    j =>
                    {
                        j.HasKey("Employee", "Project").HasName("projectEmployee_pkey");
                        j.ToTable("projectEmployee");
                        j.IndexerProperty<int>("Employee").HasColumnName("employee");
                        j.IndexerProperty<int>("Project").HasColumnName("project");
                    });
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("message_pkey");

            entity.ToTable("message");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Employee).HasColumnName("employee");
            entity.Property(e => e.Task).HasColumnName("task");
            entity.Property(e => e.Text)
                .HasColumnType("character varying")
                .HasColumnName("text");

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("message_employee_fkey");

            entity.HasOne(d => d.TaskNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.Task)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("message_task_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notification_pkey");

            entity.ToTable("notification");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Employee).HasColumnName("employee");
            entity.Property(e => e.New)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("new");
            entity.Property(e => e.Text)
                .HasColumnType("character varying")
                .HasColumnName("text");

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notification_employee_fkey");
        });

        modelBuilder.Entity<Phase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("phase_pkey");

            entity.ToTable("phase");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Project).HasColumnName("project");

            entity.HasOne(d => d.ProjectNavigation).WithMany(p => p.Phases)
                .HasForeignKey(d => d.Project)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("phase_project_fkey");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("post_pkey");

            entity.ToTable("post");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Department).HasColumnName("department");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UserGroup)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("userGroup");

            entity.HasOne(d => d.DepartmentNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.Department)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_post_of_department");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("project_pkey");

            entity.ToTable("project");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.CompletDate).HasColumnName("completDate");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.ToTable("task");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.DateComplet).HasColumnName(" dateComplet");
            entity.Property(e => e.Executor).HasColumnName("executor");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Phase).HasColumnName("phase");
            entity.Property(e => e.Responsible).HasColumnName("responsible");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("1")
                .HasColumnName("status");

            entity.HasOne(d => d.ExecutorNavigation).WithMany(p => p.TaskExecutorNavigations)
                .HasForeignKey(d => d.Executor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_executor_fkey");

            entity.HasOne(d => d.PhaseNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.Phase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_phase_fkey");

            entity.HasOne(d => d.ResponsibleNavigation).WithMany(p => p.TaskResponsibleNavigations)
                .HasForeignKey(d => d.Responsible)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_responsible_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_status_fkey");

            entity.HasMany(d => d.DependentTasks).WithMany(p => p.MainTasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskLink",
                    r => r.HasOne<Task>().WithMany()
                        .HasForeignKey("DependentTask")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("taskLink_dependentTask_fkey"),
                    l => l.HasOne<Task>().WithMany()
                        .HasForeignKey("MainTask")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("taskLink_mainTask_fkey"),
                    j =>
                    {
                        j.HasKey("MainTask", "DependentTask").HasName("taskLink_pkey");
                        j.ToTable("taskLink");
                        j.IndexerProperty<int>("MainTask").HasColumnName("mainTask");
                        j.IndexerProperty<int>("DependentTask").HasColumnName("dependentTask");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
