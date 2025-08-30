using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_raid_positions_to_raid_participant_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "raid_number",
                schema: "portal",
                table: "raid_participant",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "raid_party_number",
                schema: "portal",
                table: "raid_participant",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "raid_party_position_number",
                schema: "portal",
                table: "raid_participant",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_raid_participant_raid_number_raid_party_number_raid_party_p~",
                schema: "portal",
                table: "raid_participant",
                columns: new[] { "raid_number", "raid_party_number", "raid_party_position_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_raid_participant_raid_number_raid_party_number_raid_party_p~",
                schema: "portal",
                table: "raid_participant");

            migrationBuilder.DropColumn(
                name: "raid_number",
                schema: "portal",
                table: "raid_participant");

            migrationBuilder.DropColumn(
                name: "raid_party_number",
                schema: "portal",
                table: "raid_participant");

            migrationBuilder.DropColumn(
                name: "raid_party_position_number",
                schema: "portal",
                table: "raid_participant");
        }
    }
}
