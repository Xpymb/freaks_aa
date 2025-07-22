using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Raid_Tables_Added_CreatorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                schema: "portal",
                table: "raid_screenshot",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                schema: "portal",
                table: "raid_participant",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                schema: "portal",
                table: "raid_loot",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creator_id",
                schema: "portal",
                table: "raid_screenshot");

            migrationBuilder.DropColumn(
                name: "creator_id",
                schema: "portal",
                table: "raid_participant");

            migrationBuilder.DropColumn(
                name: "creator_id",
                schema: "portal",
                table: "raid_loot");
        }
    }
}
