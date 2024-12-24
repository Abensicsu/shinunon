using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddExamScoreCachedExamScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "cached_exam_score",
                table: "exam_executions",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cached_exam_score",
                table: "exam_executions");
        }
    }
}
