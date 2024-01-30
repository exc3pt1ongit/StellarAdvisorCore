using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarAdvisorCore.Migrations
{
    /// <inheritdoc />
    public partial class PatchV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "MutedById",
                table: "MutedUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MutedById",
                table: "MutedUsers");
        }
    }
}
