using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class QuestionIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_exam_executions_questions_question_id",
                table: "exam_executions");

            migrationBuilder.AlterColumn<int>(
                name: "question_id",
                table: "exam_executions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_exam_executions_questions_question_id",
                table: "exam_executions",
                column: "question_id",
                principalTable: "questions",
                principalColumn: "question_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_exam_executions_questions_question_id",
                table: "exam_executions");

            migrationBuilder.AlterColumn<int>(
                name: "question_id",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_exam_executions_questions_question_id",
                table: "exam_executions",
                column: "question_id",
                principalTable: "questions",
                principalColumn: "question_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
