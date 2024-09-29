using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class updateExamExecution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "wrong_answers",
                table: "exam_executions",
                newName: "wrong_answers_num");

            migrationBuilder.RenameColumn(
                name: "correct_answers",
                table: "exam_executions",
                newName: "partially_correct_answers_num");

            migrationBuilder.AddColumn<int>(
                name: "correct_answers_num",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "correct_answers_num",
                table: "exam_executions");

            migrationBuilder.RenameColumn(
                name: "wrong_answers_num",
                table: "exam_executions",
                newName: "wrong_answers");

            migrationBuilder.RenameColumn(
                name: "partially_correct_answers_num",
                table: "exam_executions",
                newName: "correct_answers");
        }
    }
}
