using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_raid_participant_indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_raid_participant_participant_id",
                schema: "portal",
                table: "raid_participant");

            migrationBuilder.DropIndex(
                name: "IX_raid_participant_raid_number_raid_party_number_raid_party_p~",
                schema: "portal",
                table: "raid_participant");

            migrationBuilder.CreateIndex(
                name: "IX_raid_participant_participant_id",
                schema: "portal",
                table: "raid_participant",
                column: "participant_id");

            migrationBuilder.CreateIndex(
                name: "IX_raid_participant_raid_id_participant_id_raid_number_raid_pa~",
                schema: "portal",
                table: "raid_participant",
                columns: new[] { "raid_id", "participant_id", "raid_number", "raid_party_number", "raid_party_position_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_raid_participant_participant_id",
                schema: "portal",
                table: "raid_participant");

            migrationBuilder.DropIndex(
                name: "IX_raid_participant_raid_id_participant_id_raid_number_raid_pa~",
                schema: "portal",
                table: "raid_participant");

            migrationBuilder.CreateIndex(
                name: "IX_raid_participant_participant_id",
                schema: "portal",
                table: "raid_participant",
                column: "participant_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_raid_participant_raid_number_raid_party_number_raid_party_p~",
                schema: "portal",
                table: "raid_participant",
                columns: new[] { "raid_number", "raid_party_number", "raid_party_position_number" },
                unique: true);
        }
    }
}
