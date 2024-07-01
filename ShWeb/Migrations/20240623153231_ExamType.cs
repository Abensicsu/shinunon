using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class ExamType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "exam_repeat_number",
                table: "exam_executions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "exam_type",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "initiated_exam",
                table: "exam_executions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exam_repeat_number",
                table: "exam_executions");

            migrationBuilder.DropColumn(
                name: "exam_type",
                table: "exam_executions");

            migrationBuilder.DropColumn(
                name: "initiated_exam",
                table: "exam_executions");
        }
    }
}
