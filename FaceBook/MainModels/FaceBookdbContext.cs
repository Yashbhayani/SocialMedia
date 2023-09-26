using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FaceBook.MainModels
{
    public partial class FaceBookdbContext : DbContext
    {
        public FaceBookdbContext()
        {
        }

        public FaceBookdbContext(DbContextOptions<FaceBookdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CommentTable> CommentTables { get; set; }
        public virtual DbSet<FriendRequestTable> FriendRequestTables { get; set; }
        public virtual DbSet<LikeTable> LikeTables { get; set; }
        public virtual DbSet<PostTable> PostTables { get; set; }
        public virtual DbSet<UserTable> UserTables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(local);initial catalog=FaceBookdb; trusted_connection=yes;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CommentTable>(entity =>
            {
                entity.ToTable("CommentTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("comment");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("_createdAt")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<FriendRequestTable>(entity =>
            {
                entity.ToTable("FriendRequestTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("_createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReceiverUserId).HasColumnName("ReceiverUserID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Pending')");
            });

            modelBuilder.Entity<LikeTable>(entity =>
            {
                entity.ToTable("LikeTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("_createdAt")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PostTable>(entity =>
            {
                entity.ToTable("PostTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("_createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("image");
            });

            modelBuilder.Entity<UserTable>(entity =>
            {
                entity.ToTable("UserTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("_createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
