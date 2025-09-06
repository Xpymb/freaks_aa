using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class rename_raid_screenshot_url_to_screenshot_uri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "screenshot_url",
                schema: "portal",
                table: "raid_screenshot",
                newName: "screenshot_uri");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "screenshot_uri",
                schema: "portal",
                table: "raid_screenshot",
                newName: "screenshot_url");
        }
    }
}
