using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Alter_BossLoot_Added_ItemDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "item_description",
                schema: "portal",
                table: "boss_loot",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "item_description",
                schema: "portal",
                table: "boss_loot");
        }
    }
}
