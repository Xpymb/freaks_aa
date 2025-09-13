using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fixraidlootfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_raid_loot_loot_item_id",
                schema: "portal",
                table: "raid_loot");

            migrationBuilder.CreateIndex(
                name: "IX_raid_loot_loot_item_id",
                schema: "portal",
                table: "raid_loot",
                column: "loot_item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_raid_loot_loot_item_id",
                schema: "portal",
                table: "raid_loot");

            migrationBuilder.CreateIndex(
                name: "IX_raid_loot_loot_item_id",
                schema: "portal",
                table: "raid_loot",
                column: "loot_item_id",
                unique: true);
        }
    }
}
