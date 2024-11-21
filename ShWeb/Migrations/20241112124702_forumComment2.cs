using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class forumComment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "question",
                table: "forum_questions",
                newName: "forum_question_text");

            migrationBuilder.AddColumn<string>(
                name: "forum_question_description",
                table: "forum_questions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "forum_question_description",
                table: "forum_questions");

            migrationBuilder.RenameColumn(
                name: "forum_question_text",
                table: "forum_questions",
                newName: "question");
        }
    }
}
