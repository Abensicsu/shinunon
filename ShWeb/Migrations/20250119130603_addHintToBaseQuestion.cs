using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShWeb.Migrations
{
    /// <inheritdoc />
    public partial class addHintToBaseQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "hint",
                table: "base_questions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hint",
                table: "base_questions");
        }
    }
}
