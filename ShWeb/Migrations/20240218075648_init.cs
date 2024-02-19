using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_type = table.Column<int>(type: "integer", nullable: false),
                    subject_name = table.Column<string>(type: "text", nullable: true),
                    ordinal = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subjects", x => x.subject_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_text = table.Column<string>(type: "text", nullable: true),
                    question_type = table.Column<int>(type: "integer", nullable: false),
                    subject_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_questions", x => x.question_id);
                    table.ForeignKey(
                        name: "fk_questions_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_executions",
                columns: table => new
                {
                    exam_execution_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    subject_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exam_executions", x => x.exam_execution_id);
                    table.ForeignKey(
                        name: "fk_exam_executions_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exam_executions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "forum_questions",
                columns: table => new
                {
                    forum_question_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    forum_question_type = table.Column<int>(type: "integer", nullable: false),
                    question = table.Column<string>(type: "text", nullable: true),
                    subject_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_forum_questions", x => x.forum_question_id);
                    table.ForeignKey(
                        name: "fk_forum_questions_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id");
                    table.ForeignKey(
                        name: "fk_forum_questions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    answer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    answer_text = table.Column<string>(type: "text", nullable: true),
                    question_id = table.Column<int>(type: "integer", nullable: false),
                    is_correct_answer = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_answers", x => x.answer_id);
                    table.ForeignKey(
                        name: "fk_answers_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "forum_comments",
                columns: table => new
                {
                    forum_comment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    comment = table.Column<string>(type: "text", nullable: true),
                    forum_question_id = table.Column<int>(type: "integer", nullable: false),
                    parent_comment_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_forum_comments", x => x.forum_comment_id);
                    table.ForeignKey(
                        name: "fk_forum_comments_forum_comments_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalTable: "forum_comments",
                        principalColumn: "forum_comment_id");
                    table.ForeignKey(
                        name: "fk_forum_comments_forum_questions_forum_question_id",
                        column: x => x.forum_question_id,
                        principalTable: "forum_questions",
                        principalColumn: "forum_question_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_forum_comments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_answers",
                columns: table => new
                {
                    exam_answer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_id = table.Column<int>(type: "integer", nullable: false),
                    answer_id = table.Column<int>(type: "integer", nullable: true),
                    text_answer = table.Column<string>(type: "text", nullable: true),
                    exam_execution_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exam_answers", x => x.exam_answer_id);
                    table.ForeignKey(
                        name: "fk_exam_answers_answers_answer_id",
                        column: x => x.answer_id,
                        principalTable: "answers",
                        principalColumn: "answer_id");
                    table.ForeignKey(
                        name: "fk_exam_answers_exam_executions_exam_execution_id",
                        column: x => x.exam_execution_id,
                        principalTable: "exam_executions",
                        principalColumn: "exam_execution_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exam_answers_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_answers_question_id",
                table: "answers",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_answers_answer_id",
                table: "exam_answers",
                column: "answer_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_answers_exam_execution_id",
                table: "exam_answers",
                column: "exam_execution_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_answers_question_id",
                table: "exam_answers",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_executions_subject_id",
                table: "exam_executions",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_executions_user_id",
                table: "exam_executions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_forum_comments_forum_question_id",
                table: "forum_comments",
                column: "forum_question_id");

            migrationBuilder.CreateIndex(
                name: "ix_forum_comments_parent_comment_id",
                table: "forum_comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "ix_forum_comments_user_id",
                table: "forum_comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_forum_questions_subject_id",
                table: "forum_questions",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_forum_questions_user_id",
                table: "forum_questions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_questions_subject_id",
                table: "questions",
                column: "subject_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_answers");

            migrationBuilder.DropTable(
                name: "forum_comments");

            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "exam_executions");

            migrationBuilder.DropTable(
                name: "forum_questions");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "subjects");
        }
    }
}
