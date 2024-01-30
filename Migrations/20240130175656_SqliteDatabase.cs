using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarAdvisorCore.Migrations
{
    /// <inheritdoc />
    public partial class SqliteDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MutedUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MemberId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    MutedReason = table.Column<string>(type: "TEXT", nullable: true),
                    MutedExpiration = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MutedUsers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MutedUsers");
        }
    }
}
