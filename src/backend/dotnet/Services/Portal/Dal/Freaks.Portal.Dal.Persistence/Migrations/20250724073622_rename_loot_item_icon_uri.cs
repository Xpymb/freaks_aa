using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class rename_loot_item_icon_uri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "icon_uri",
                schema: "portal",
                table: "loot_item",
                newName: "icon_url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "roles",
                schema: "users",
                table: "users");
        }
    }
}
