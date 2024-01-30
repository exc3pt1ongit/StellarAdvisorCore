using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarAdvisorCore.Migrations
{
    /// <inheritdoc />
    public partial class PatchV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "MutedUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "MutedUsers");
        }
    }
}
