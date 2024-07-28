using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "__efmigrationshistory",
            //    columns: table => new
            //    {
            //        MigrationId = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_0900_ai_ci")
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ProductVersion = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PRIMARY", x => x.MigrationId);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4")
            //    .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "department",
                columns: table => new
                {
                    dp_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dp_name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.dp_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    pt_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pt_name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dob = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.pt_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "doctors",
                columns: table => new
                {
                    doctor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dp_id = table.Column<int>(type: "int", nullable: true),
                    fname = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lname = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.doctor_id);
                    table.ForeignKey(
                        name: "dp_id",
                        column: x => x.dp_id,
                        principalTable: "department",
                        principalColumn: "dp_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "visits",
                columns: table => new
                {
                    visit_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pt_id = table.Column<int>(type: "int", nullable: true),
                    doctor_id = table.Column<int>(type: "int", nullable: true),
                    visit_date = table.Column<DateOnly>(type: "date", nullable: true),
                    purpose = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.visit_id);
                    table.ForeignKey(
                        name: "visits_ibfk_1",
                        column: x => x.pt_id,
                        principalTable: "patients",
                        principalColumn: "pt_id");
                    table.ForeignKey(
                        name: "visits_ibfk_2",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "doctor_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    test_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pt_id = table.Column<int>(type: "int", nullable: true),
                    test_type = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    doctor_id = table.Column<int>(type: "int", nullable: true),
                    visit_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.test_id);
                    table.ForeignKey(
                        name: "tests_ibfk_1",
                        column: x => x.pt_id,
                        principalTable: "patients",
                        principalColumn: "pt_id");
                    table.ForeignKey(
                        name: "tests_ibfk_2",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "doctor_id");
                    table.ForeignKey(
                        name: "tests_ibfk_3",
                        column: x => x.visit_id,
                        principalTable: "visits",
                        principalColumn: "visit_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "dp_id",
                table: "department",
                column: "dp_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "doctor_id",
                table: "doctors",
                column: "doctor_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "dp_id_idx",
                table: "doctors",
                column: "dp_id");

            migrationBuilder.CreateIndex(
                name: "pt_id",
                table: "patients",
                column: "pt_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "doctor_id1",
                table: "tests",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "pt_id1",
                table: "tests",
                column: "pt_id");

            migrationBuilder.CreateIndex(
                name: "test_id",
                table: "tests",
                column: "test_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "visit_id",
                table: "tests",
                column: "visit_id");

            migrationBuilder.CreateIndex(
                name: "doctor_id2",
                table: "visits",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "pt_id2",
                table: "visits",
                column: "pt_id");

            migrationBuilder.CreateIndex(
                name: "visit_id1",
                table: "visits",
                column: "visit_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__efmigrationshistory");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "visits");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "doctors");

            migrationBuilder.DropTable(
                name: "department");
        }
    }
}
