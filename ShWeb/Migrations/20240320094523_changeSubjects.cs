using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class changeSubjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "subject_type",
                table: "subjects",
                newName: "book_id");

            migrationBuilder.AddColumn<int>(
                name: "correct_answers",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "plan_exam_id",
                table: "exam_executions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "question_id",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "questions_answered",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "wrong_answers",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    book_name = table.Column<string>(type: "text", nullable: true),
                    book_type = table.Column<int>(type: "integer", nullable: false),
                    has_sub_subject = table.Column<bool>(type: "boolean", nullable: false),
                    book_order = table.Column<int>(type: "integer", nullable: true),
                    subject_num = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_books", x => x.book_id);
                });

            migrationBuilder.CreateTable(
                name: "plan_exams",
                columns: table => new
                {
                    plan_exam_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exam_frequency = table.Column<int>(type: "integer", nullable: false),
                    subject_num = table.Column<int>(type: "integer", nullable: false),
                    sub_subject_num = table.Column<int>(type: "integer", nullable: false),
                    questions_amount = table.Column<int>(type: "integer", nullable: false),
                    from_subject_id = table.Column<int>(type: "integer", nullable: false),
                    to_subject_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plan_exams", x => x.plan_exam_id);
                    table.ForeignKey(
                        name: "fk_plan_exams_subjects_from_subject_id",
                        column: x => x.from_subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_plan_exams_subjects_to_subject_id",
                        column: x => x.to_subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_plan_exams_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_subjects_book_id",
                table: "subjects",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_executions_plan_exam_id",
                table: "exam_executions",
                column: "plan_exam_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_executions_question_id",
                table: "exam_executions",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_plan_exams_from_subject_id",
                table: "plan_exams",
                column: "from_subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_plan_exams_to_subject_id",
                table: "plan_exams",
                column: "to_subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_plan_exams_user_id",
                table: "plan_exams",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_exam_executions_plan_exams_plan_exam_id",
                table: "exam_executions",
                column: "plan_exam_id",
                principalTable: "plan_exams",
                principalColumn: "plan_exam_id");

            migrationBuilder.AddForeignKey(
                name: "fk_exam_executions_questions_question_id",
                table: "exam_executions",
                column: "question_id",
                principalTable: "questions",
                principalColumn: "question_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_subjects_books_book_id",
                table: "subjects",
                column: "book_id",
                principalTable: "books",
                principalColumn: "book_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_exam_executions_plan_exams_plan_exam_id",
                table: "exam_executions");

            migrationBuilder.DropForeignKey(
                name: "fk_exam_executions_questions_question_id",
                table: "exam_executions");

            migrationBuilder.DropForeignKey(
                name: "fk_subjects_books_book_id",
                table: "subjects");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "plan_exams");

            migrationBuilder.DropIndex(
                name: "ix_subjects_book_id",
                table: "subjects");

            migrationBuilder.DropIndex(
                name: "ix_exam_executions_plan_exam_id",
                table: "exam_executions");

            migrationBuilder.DropIndex(
                name: "ix_exam_executions_question_id",
                table: "exam_executions");

            migrationBuilder.DropColumn(
                name: "correct_answers",
                table: "exam_executions");

            migrationBuilder.DropColumn(
                name: "plan_exam_id",
                table: "exam_executions");

            migrationBuilder.DropColumn(
                name: "question_id",
                table: "exam_executions");

            migrationBuilder.DropColumn(
                name: "questions_answered",
                table: "exam_executions");

            migrationBuilder.DropColumn(
                name: "wrong_answers",
                table: "exam_executions");

            migrationBuilder.RenameColumn(
                name: "book_id",
                table: "subjects",
                newName: "subject_type");
        }
    }
}
