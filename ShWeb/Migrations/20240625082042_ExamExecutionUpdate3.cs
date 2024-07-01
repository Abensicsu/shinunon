using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class ExamExecutionUpdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "initiated_exam",
                table: "exam_executions");

            migrationBuilder.DropColumn(
                name: "repeat_exam",
                table: "exam_executions");

            migrationBuilder.AddColumn<int>(
                name: "exam_type",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exam_type",
                table: "exam_executions");

            migrationBuilder.AddColumn<bool>(
                name: "initiated_exam",
                table: "exam_executions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "repeat_exam",
                table: "exam_executions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
