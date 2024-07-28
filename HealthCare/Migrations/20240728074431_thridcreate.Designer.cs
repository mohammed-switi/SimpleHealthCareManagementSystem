﻿// <auto-generated />
using System;
using HealthCare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HealthCare.Migrations
{
    [DbContext(typeof(HealthcaredbContext))]
    [Migration("20240728074431_thridcreate")]
    partial class thridcreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("HealthCare.Models.Department", b =>
                {
                    b.Property<int>("DpId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("dp_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DpId"));

                    b.Property<string>("DpName")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("dp_name");

                    b.Property<string>("Location")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("location");

                    b.HasKey("DpId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "DpId" }, "dp_id")
                        .IsUnique();

                    b.ToTable("department", (string)null);
                });

            modelBuilder.Entity("HealthCare.Models.Doctor", b =>
                {
                    b.Property<int>("DoctorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("doctor_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DoctorId"));

                    b.Property<int?>("DpId")
                        .HasColumnType("int")
                        .HasColumnName("dp_id");

                    b.Property<string>("Fname")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("fname");

                    b.Property<string>("Lname")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("lname");

                    b.HasKey("DoctorId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "DoctorId" }, "doctor_id")
                        .IsUnique();

                    b.HasIndex(new[] { "DpId" }, "dp_id_idx");

                    b.ToTable("doctors", (string)null);
                });

            modelBuilder.Entity("HealthCare.Models.Efmigrationshistory", b =>
                {
                    b.Property<string>("MigrationId")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("ProductVersion")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("MigrationId")
                        .HasName("PRIMARY");

                    b.ToTable("__efmigrationshistory", (string)null);
                });

            modelBuilder.Entity("HealthCare.Models.Patient", b =>
                {
                    b.Property<int>("PtId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("pt_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("PtId"));

                    b.Property<DateOnly?>("Dob")
                        .HasColumnType("date")
                        .HasColumnName("dob");

                    b.Property<string>("Gender")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("gender");

                    b.Property<string>("PtName")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("pt_name");

                    b.HasKey("PtId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "PtId" }, "pt_id")
                        .IsUnique();

                    b.ToTable("patients", (string)null);
                });

            modelBuilder.Entity("HealthCare.Models.Test", b =>
                {
                    b.Property<int>("TestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("test_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("TestId"));

                    b.Property<int?>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnName("doctor_id");

                    b.Property<int?>("PtId")
                        .HasColumnType("int")
                        .HasColumnName("pt_id");

                    b.Property<string>("TestType")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("test_type");

                    b.Property<int?>("VisitId")
                        .HasColumnType("int")
                        .HasColumnName("visit_id");

                    b.HasKey("TestId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "DoctorId" }, "doctor_id")
                        .HasDatabaseName("doctor_id1");

                    b.HasIndex(new[] { "PtId" }, "pt_id")
                        .HasDatabaseName("pt_id1");

                    b.HasIndex(new[] { "TestId" }, "test_id")
                        .IsUnique();

                    b.HasIndex(new[] { "VisitId" }, "visit_id");

                    b.ToTable("tests", (string)null);
                });

            modelBuilder.Entity("HealthCare.Models.Visit", b =>
                {
                    b.Property<int>("VisitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("visit_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("VisitId"));

                    b.Property<int?>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnName("doctor_id");

                    b.Property<int?>("PtId")
                        .HasColumnType("int")
                        .HasColumnName("pt_id");

                    b.Property<string>("Purpose")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("purpose");

                    b.Property<DateOnly?>("VisitDate")
                        .HasColumnType("date")
                        .HasColumnName("visit_date");

                    b.HasKey("VisitId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "DoctorId" }, "doctor_id")
                        .HasDatabaseName("doctor_id2");

                    b.HasIndex(new[] { "PtId" }, "pt_id")
                        .HasDatabaseName("pt_id2");

                    b.HasIndex(new[] { "VisitId" }, "visit_id")
                        .IsUnique()
                        .HasDatabaseName("visit_id1");

                    b.ToTable("visits", (string)null);
                });

            modelBuilder.Entity("HealthCare.Models.Doctor", b =>
                {
                    b.HasOne("HealthCare.Models.Department", "Dp")
                        .WithMany("Doctors")
                        .HasForeignKey("DpId")
                        .HasConstraintName("dp_id");

                    b.Navigation("Dp");
                });

            modelBuilder.Entity("HealthCare.Models.Test", b =>
                {
                    b.HasOne("HealthCare.Models.Doctor", "Doctor")
                        .WithMany("Tests")
                        .HasForeignKey("DoctorId")
                        .HasConstraintName("tests_ibfk_2");

                    b.HasOne("HealthCare.Models.Patient", "Pt")
                        .WithMany("Tests")
                        .HasForeignKey("PtId")
                        .HasConstraintName("tests_ibfk_1");

                    b.HasOne("HealthCare.Models.Visit", "Visit")
                        .WithMany("Tests")
                        .HasForeignKey("VisitId")
                        .HasConstraintName("tests_ibfk_3");

                    b.Navigation("Doctor");

                    b.Navigation("Pt");

                    b.Navigation("Visit");
                });

            modelBuilder.Entity("HealthCare.Models.Visit", b =>
                {
                    b.HasOne("HealthCare.Models.Doctor", "Doctor")
                        .WithMany("Visits")
                        .HasForeignKey("DoctorId")
                        .HasConstraintName("visits_ibfk_2");

                    b.HasOne("HealthCare.Models.Patient", "Pt")
                        .WithMany("Visits")
                        .HasForeignKey("PtId")
                        .HasConstraintName("visits_ibfk_1");

                    b.Navigation("Doctor");

                    b.Navigation("Pt");
                });

            modelBuilder.Entity("HealthCare.Models.Department", b =>
                {
                    b.Navigation("Doctors");
                });

            modelBuilder.Entity("HealthCare.Models.Doctor", b =>
                {
                    b.Navigation("Tests");

                    b.Navigation("Visits");
                });

            modelBuilder.Entity("HealthCare.Models.Patient", b =>
                {
                    b.Navigation("Tests");

                    b.Navigation("Visits");
                });

            modelBuilder.Entity("HealthCare.Models.Visit", b =>
                {
                    b.Navigation("Tests");
                });
#pragma warning restore 612, 618
        }
    }
}
