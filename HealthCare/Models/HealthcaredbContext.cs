using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Models
{
    public class HealthcaredbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
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
        public DbSet<LogEntry> LogEntries { get; set; }



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

            
        }

     
    }
}
