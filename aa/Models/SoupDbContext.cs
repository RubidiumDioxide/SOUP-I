using System;
using System.Collections.Generic;
using aa.Models;
using Microsoft.EntityFrameworkCore;

namespace aa.Models;

public partial class SoupDbContext : DbContext
{
    public SoupDbContext(DbContextOptions<SoupDbContext> options) : base(options) 
    {
    }

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Repository> Repositories { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ACTION__3214EC2785BEDEED");

            entity.ToTable("ACTION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActorId).HasColumnName("ActorID");
            entity.Property(e => e.Commit).HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");

            entity.HasOne(d => d.Actor).WithMany(p => p.Actions)
                .HasForeignKey(d => d.ActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ACTION__ActorID__52CE3E04");

            entity.HasOne(d => d.Project).WithMany(p => p.Actions)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ACTION__ProjectI__51DA19CB");

            entity.HasOne(d => d.Task).WithMany(p => p.Actions)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ACTION__TaskID__53C2623D");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NOTIFICA__3214EC2708B67C8E");

            entity.ToTable("NOTIFICATION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.Project).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NOTIFICAT__Proje__457442E6");

            entity.HasOne(d => d.Receiver).WithMany(p => p.NotificationReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NOTIFICAT__Recei__475C8B58");

            entity.HasOne(d => d.Sender).WithMany(p => p.NotificationSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NOTIFICAT__Sende__4668671F");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PROJECT__3214EC27386BE2BB");

            entity.ToTable("PROJECT");

            entity.HasIndex(e => e.Name, "UQ__PROJECT__737584F6A2F3BDDC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateBegan).HasColumnType("datetime");
            entity.Property(e => e.DateDeadline).HasColumnType("datetime");
            entity.Property(e => e.DateFinished).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.IsPrivate).HasColumnName("isPrivate");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.CreatorNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Creator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PROJECT__Creator__3726238F");
        });

        modelBuilder.Entity<Repository>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__REPOSITO__3214EC270DAC6040");

            entity.ToTable("REPOSITORY");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.GithubCreator).HasMaxLength(200);
            entity.Property(e => e.GithubName).HasMaxLength(200);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Repository)
                .HasForeignKey<Repository>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__REPOSITORY__ID__3BEAD8AC");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TASK__3214EC277280868F");

            entity.ToTable("TASK");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssigneeId).HasColumnName("AssigneeID");
            entity.Property(e => e.CreatorId).HasColumnName("CreatorID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.Assignee).WithMany(p => p.TaskAssignees)
                .HasForeignKey(d => d.AssigneeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TASK__AssigneeID__4E0988E7");

            entity.HasOne(d => d.Creator).WithMany(p => p.TaskCreators)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TASK__CreatorID__4D1564AE");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TASK__ProjectID__4C214075");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TEAM__3214EC277E3667D0");

            entity.ToTable("TEAM");

            entity.HasIndex(e => new { e.ProjectId, e.Role }, "IX_UC_SingleManagers")
                .IsUnique()
                .HasFilter("([Role] IN ('Руководитель проекта', 'Руководитель отдела дизайна', 'Руководитель отдела разработки', 'Руководитель отдела внедрения и тестирования', 'Руководитель отдела информационной безопасности', 'Руководитель отдела аналитики'))");

            entity.HasIndex(e => new { e.UserId, e.ProjectId, e.Role }, "UQ__TEAM__3E337201C1029B7F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Project).WithMany(p => p.Teams)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TEAM__ProjectID__40AF8DC9");

            entity.HasOne(d => d.User).WithMany(p => p.Teams)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TEAM__UserID__3FBB6990");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER__3214EC27D0292FE2");

            entity.ToTable("USER");

            entity.HasIndex(e => e.Name, "UQ__USER__737584F6E452BCA1").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Password).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
