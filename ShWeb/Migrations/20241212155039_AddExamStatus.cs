using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddExamStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "exam_status",
                table: "exam_executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exam_status",
                table: "exam_executions");
        }
    }
}
