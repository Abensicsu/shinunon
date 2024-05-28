using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class Updateplanexam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_exam_executions_subjects_subject_id",
                table: "exam_executions");

            migrationBuilder.DropForeignKey(
                name: "fk_plan_exams_subjects_from_subject_id",
                table: "plan_exams");

            migrationBuilder.DropForeignKey(
                name: "fk_plan_exams_subjects_to_subject_id",
                table: "plan_exams");

            migrationBuilder.DropColumn(
                name: "name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "subject_num",
                table: "books");

            migrationBuilder.RenameColumn(
                name: "subject_id",
                table: "exam_executions",
                newName: "to_subject_id");

            migrationBuilder.RenameIndex(
                name: "ix_exam_executions_subject_id",
                table: "exam_executions",
                newName: "ix_exam_executions_to_subject_id");

            migrationBuilder.AddColumn<int>(
                name: "sub_subject_count",
                table: "subjects",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "from_subject_id",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "book_order",
                table: "books",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_exam_executions_from_subject_id",
                table: "exam_executions",
                column: "from_subject_id");

            migrationBuilder.AddForeignKey(
                name: "fk_exam_executions_subjects_from_subject_id",
                table: "exam_executions",
                column: "from_subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_exam_executions_subjects_to_subject_id",
                table: "exam_executions",
                column: "to_subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_plan_exams_subjects_from_subject_id",
                table: "plan_exams",
                column: "from_subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_plan_exams_subjects_to_subject_id",
                table: "plan_exams",
                column: "to_subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_exam_executions_subjects_from_subject_id",
                table: "exam_executions");

            migrationBuilder.DropForeignKey(
                name: "fk_exam_executions_subjects_to_subject_id",
                table: "exam_executions");

            migrationBuilder.DropForeignKey(
                name: "fk_plan_exams_subjects_from_subject_id",
                table: "plan_exams");

            migrationBuilder.DropForeignKey(
                name: "fk_plan_exams_subjects_to_subject_id",
                table: "plan_exams");

            migrationBuilder.DropIndex(
                name: "ix_exam_executions_from_subject_id",
                table: "exam_executions");

            migrationBuilder.DropColumn(
                name: "sub_subject_count",
                table: "subjects");

            migrationBuilder.DropColumn(
                name: "from_subject_id",
                table: "exam_executions");

            migrationBuilder.RenameColumn(
                name: "to_subject_id",
                table: "exam_executions",
                newName: "subject_id");

            migrationBuilder.RenameIndex(
                name: "ix_exam_executions_to_subject_id",
                table: "exam_executions",
                newName: "ix_exam_executions_subject_id");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "book_order",
                table: "books",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "subject_num",
                table: "books",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "fk_exam_executions_subjects_subject_id",
                table: "exam_executions",
                column: "subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_plan_exams_subjects_from_subject_id",
                table: "plan_exams",
                column: "from_subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_plan_exams_subjects_to_subject_id",
                table: "plan_exams",
                column: "to_subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
