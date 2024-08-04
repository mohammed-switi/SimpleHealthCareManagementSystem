using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Migrations
{
    /// <inheritdoc />
    public partial class serilog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogLevel",
                table: "LogEntries");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "LogEntries",
                newName: "TimeStamp");

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "LogEntries",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LogEvent",
                table: "LogEntries",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MessageTemplate",
                table: "LogEntries",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "LogEntries",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "LogEvent",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "MessageTemplate",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "LogEntries");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "LogEntries",
                newName: "Timestamp");

            migrationBuilder.AddColumn<int>(
                name: "LogLevel",
                table: "LogEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
