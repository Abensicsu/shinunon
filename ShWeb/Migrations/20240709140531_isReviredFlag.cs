using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class isReviredFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_reviewed",
                table: "exam_executions",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_reviewed",
                table: "exam_executions");
        }
    }
}
