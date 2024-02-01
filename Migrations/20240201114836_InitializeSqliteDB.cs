using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarAdvisorCore.Migrations
{
    /// <inheritdoc />
    public partial class InitializeSqliteDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DiscordUserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Settlement = table.Column<string>(type: "TEXT", nullable: true),
                    Faction = table.Column<string>(type: "TEXT", nullable: true),
                    FactionRole = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MutedUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MemberId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    MutedReason = table.Column<string>(type: "TEXT", nullable: true),
                    MutedExpiration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MutedById = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "Characters");

            migrationBuilder.DropTable(
                name: "MutedUsers");
        }
    }
}
