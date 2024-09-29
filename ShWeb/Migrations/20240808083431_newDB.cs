using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class newDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    book_name = table.Column<string>(type: "text", nullable: true),
                    hebrew_book_name = table.Column<string>(type: "text", nullable: true),
                    book_type = table.Column<int>(type: "integer", nullable: false),
                    has_sub_subject = table.Column<bool>(type: "boolean", nullable: false),
                    book_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_books", x => x.book_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    user_settings_memory_level = table.Column<int>(type: "integer", nullable: true),
                    user_settings_default_question_count = table.Column<int>(type: "integer", nullable: true),
                    user_settings_default_time_exam = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_name = table.Column<string>(type: "text", nullable: true),
                    ordinal = table.Column<int>(type: "integer", nullable: true),
                    sub_subject_count = table.Column<int>(type: "integer", nullable: true),
                    book_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subjects", x => x.subject_id);
                    table.ForeignKey(
                        name: "fk_subjects_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "base_questions",
                columns: table => new
                {
                    base_question_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_text = table.Column<string>(type: "text", nullable: true),
                    question_type = table.Column<int>(type: "integer", nullable: false),
                    discriminator_rf = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    subject_id = table.Column<int>(type: "integer", nullable: false),
                    subject_id1 = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    original_question_id = table.Column<int>(type: "integer", nullable: true),
                    user_id1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_base_questions", x => x.base_question_id);
                    table.ForeignKey(
                        name: "fk_base_questions_base_questions_original_question_id",
                        column: x => x.original_question_id,
                        principalTable: "base_questions",
                        principalColumn: "base_question_id");
                    table.ForeignKey(
                        name: "fk_base_questions_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_base_questions_subjects_subject_id1",
                        column: x => x.subject_id1,
                        principalTable: "subjects",
                        principalColumn: "subject_id");
                    table.ForeignKey(
                        name: "fk_base_questions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_base_questions_users_user_id1",
                        column: x => x.user_id1,
                        principalTable: "users",
                        principalColumn: "user_id");
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
                name: "plan_exams",
                columns: table => new
                {
                    exam_plan_id = table.Column<int>(type: "integer", nullable: false)
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
                    table.PrimaryKey("pk_plan_exams", x => x.exam_plan_id);
                    table.ForeignKey(
                        name: "fk_plan_exams_subjects_from_subject_id",
                        column: x => x.from_subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_plan_exams_subjects_to_subject_id",
                        column: x => x.to_subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_plan_exams_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subject_texts",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "integer", nullable: false),
                    subject_text_content = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subject_texts", x => x.subject_id);
                    table.ForeignKey(
                        name: "fk_subject_texts_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    answer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    answer_text = table.Column<string>(type: "text", nullable: true),
                    is_correct_answer = table.Column<bool>(type: "boolean", nullable: false),
                    base_question_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_answers", x => x.answer_id);
                    table.ForeignKey(
                        name: "fk_answers_base_questions_base_question_id",
                        column: x => x.base_question_id,
                        principalTable: "base_questions",
                        principalColumn: "base_question_id",
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
                name: "exam_executions",
                columns: table => new
                {
                    exam_execution_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    correct_answers = table.Column<int>(type: "integer", nullable: false),
                    wrong_answers = table.Column<int>(type: "integer", nullable: false),
                    questions_answered = table.Column<int>(type: "integer", nullable: false),
                    exam_type = table.Column<int>(type: "integer", nullable: false),
                    exam_repeat_number = table.Column<int>(type: "integer", nullable: true),
                    is_reviewed = table.Column<bool>(type: "boolean", nullable: true),
                    base_question_id = table.Column<int>(type: "integer", nullable: true),
                    from_subject_id = table.Column<int>(type: "integer", nullable: false),
                    to_subject_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    plan_exam_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exam_executions", x => x.exam_execution_id);
                    table.ForeignKey(
                        name: "fk_exam_executions_base_questions_base_question_id",
                        column: x => x.base_question_id,
                        principalTable: "base_questions",
                        principalColumn: "base_question_id");
                    table.ForeignKey(
                        name: "fk_exam_executions_plan_exams_plan_exam_id",
                        column: x => x.plan_exam_id,
                        principalTable: "plan_exams",
                        principalColumn: "exam_plan_id");
                    table.ForeignKey(
                        name: "fk_exam_executions_subjects_from_subject_id",
                        column: x => x.from_subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_executions_subjects_to_subject_id",
                        column: x => x.to_subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_executions_users_user_id",
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
                    text_answer = table.Column<string>(type: "text", nullable: true),
                    time_spent = table.Column<TimeSpan>(type: "interval", nullable: false),
                    answer_id = table.Column<int>(type: "integer", nullable: true),
                    base_question_id = table.Column<int>(type: "integer", nullable: true),
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
                        name: "fk_exam_answers_base_questions_base_question_id",
                        column: x => x.base_question_id,
                        principalTable: "base_questions",
                        principalColumn: "base_question_id");
                    table.ForeignKey(
                        name: "fk_exam_answers_exam_executions_exam_execution_id",
                        column: x => x.exam_execution_id,
                        principalTable: "exam_executions",
                        principalColumn: "exam_execution_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_answers_base_question_id",
                table: "answers",
                column: "base_question_id");

            migrationBuilder.CreateIndex(
                name: "ix_base_questions_original_question_id",
                table: "base_questions",
                column: "original_question_id");

            migrationBuilder.CreateIndex(
                name: "ix_base_questions_subject_id",
                table: "base_questions",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_base_questions_subject_id1",
                table: "base_questions",
                column: "subject_id1");

            migrationBuilder.CreateIndex(
                name: "ix_base_questions_user_id",
                table: "base_questions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_base_questions_user_id1",
                table: "base_questions",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "ix_exam_answers_answer_id",
                table: "exam_answers",
                column: "answer_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_answers_base_question_id",
                table: "exam_answers",
                column: "base_question_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_answers_exam_execution_id",
                table: "exam_answers",
                column: "exam_execution_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_executions_base_question_id",
                table: "exam_executions",
                column: "base_question_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_executions_from_subject_id",
                table: "exam_executions",
                column: "from_subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_executions_plan_exam_id",
                table: "exam_executions",
                column: "plan_exam_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_executions_to_subject_id",
                table: "exam_executions",
                column: "to_subject_id");

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

            migrationBuilder.CreateIndex(
                name: "ix_subjects_book_id",
                table: "subjects",
                column: "book_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_answers");

            migrationBuilder.DropTable(
                name: "forum_comments");

            migrationBuilder.DropTable(
                name: "subject_texts");

            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "exam_executions");

            migrationBuilder.DropTable(
                name: "forum_questions");

            migrationBuilder.DropTable(
                name: "base_questions");

            migrationBuilder.DropTable(
                name: "plan_exams");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "books");
        }
    }
}
