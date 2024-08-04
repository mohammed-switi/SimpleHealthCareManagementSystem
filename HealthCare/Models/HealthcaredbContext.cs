using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Models
{
    public partial class HealthcaredbContext : IdentityDbContext
    {
        public HealthcaredbContext()
        {
        }

        public HealthcaredbContext(DbContextOptions<HealthcaredbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=healthcaredb;user=root;pwd=1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure the Identity configurations are applied

            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DpId).HasName("PRIMARY");

                entity.ToTable("department");

                entity.HasIndex(e => e.DpId, "dp_id").IsUnique();

                entity.Property(e => e.DpId).HasColumnName("dp_id");
                entity.Property(e => e.DpName)
                    .HasMaxLength(32)
                    .HasColumnName("dp_name");
                entity.Property(e => e.Location)
                    .HasMaxLength(32)
                    .HasColumnName("location");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.DoctorId).HasName("PRIMARY");

                entity.ToTable("doctors");

                entity.HasIndex(e => e.DoctorId, "doctor_id").IsUnique();

                entity.HasIndex(e => e.DpId, "dp_id_idx");

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
                entity.Property(e => e.DpId).HasColumnName("dp_id");
                entity.Property(e => e.Fname)
                    .HasMaxLength(32)
                    .HasColumnName("fname");
                entity.Property(e => e.Lname)
                    .HasMaxLength(32)
                    .HasColumnName("lname");

                entity.HasOne(d => d.Dp).WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.DpId)
                    .HasConstraintName("dp_id");
            });

            modelBuilder.Entity<Efmigrationshistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

                entity.ToTable("__efmigrationshistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);
                entity.Property(e => e.ProductVersion).HasMaxLength(32);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PtId).HasName("PRIMARY");

                entity.ToTable("patients");

                entity.HasIndex(e => e.PtId, "pt_id").IsUnique();

                entity.Property(e => e.PtId).HasColumnName("pt_id");
                entity.Property(e => e.Dob).HasColumnName("dob");
                entity.Property(e => e.Gender)
                    .HasMaxLength(32)
                    .HasColumnName("gender");
                entity.Property(e => e.PtName)
                    .HasMaxLength(32)
                    .HasColumnName("pt_name");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasKey(e => e.TestId).HasName("PRIMARY");

                entity.ToTable("tests");

                entity.HasIndex(e => e.DoctorId, "doctor_id");

                entity.HasIndex(e => e.PtId, "pt_id");

                entity.HasIndex(e => e.TestId, "test_id").IsUnique();

                entity.HasIndex(e => e.VisitId, "visit_id");

                entity.Property(e => e.TestId).HasColumnName("test_id");
                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
                entity.Property(e => e.PtId).HasColumnName("pt_id");
                entity.Property(e => e.TestType)
                    .HasMaxLength(32)
                    .HasColumnName("test_type");
                entity.Property(e => e.VisitId).HasColumnName("visit_id");

                entity.HasOne(d => d.Doctor).WithMany(p => p.Tests)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("tests_ibfk_2");

                entity.HasOne(d => d.Pt).WithMany(p => p.Tests)
                    .HasForeignKey(d => d.PtId)
                    .HasConstraintName("tests_ibfk_1");

                entity.HasOne(d => d.Visit).WithMany(p => p.Tests)
                    .HasForeignKey(d => d.VisitId)
                    .HasConstraintName("tests_ibfk_3");
            });

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasKey(e => e.VisitId).HasName("PRIMARY");

                entity.ToTable("visits");

                entity.HasIndex(e => e.DoctorId, "doctor_id");

                entity.HasIndex(e => e.PtId, "pt_id");

                entity.HasIndex(e => e.VisitId, "visit_id").IsUnique();

                entity.Property(e => e.VisitId).HasColumnName("visit_id");
                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
                entity.Property(e => e.PtId).HasColumnName("pt_id");
                entity.Property(e => e.Purpose)
                    .HasMaxLength(32)
                    .HasColumnName("purpose");
                entity.Property(e => e.VisitDate).HasColumnName("visit_date");

                entity.HasOne(d => d.Doctor).WithMany(p => p.Visits)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("visits_ibfk_2");

                entity.HasOne(d => d.Pt).WithMany(p => p.Visits)
                    .HasForeignKey(d => d.PtId)
                    .HasConstraintName("visits_ibfk_1");
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.userId);
                entity.Property(e => e.firstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.lastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.phone).HasMaxLength(15);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
