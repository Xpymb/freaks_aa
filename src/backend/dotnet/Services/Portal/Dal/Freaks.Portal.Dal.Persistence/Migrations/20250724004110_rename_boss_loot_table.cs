using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class rename_boss_loot_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_raid_loot_boss_loot_loot_id",
                schema: "portal",
                table: "raid_loot");

            migrationBuilder.DropTable(
                name: "boss_loot",
                schema: "portal");

            migrationBuilder.CreateTable(
                name: "loot_item",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    loot_type = table.Column<int>(type: "integer", nullable: false),
                    grade_type = table.Column<int>(type: "integer", nullable: false),
                    item_name = table.Column<string>(type: "text", nullable: false),
                    item_description = table.Column<string>(type: "text", nullable: false),
                    synthesis_exp = table.Column<int>(type: "integer", nullable: true),
                    icon_uri = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loot_item", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_raid_loot_loot_item_loot_id",
                schema: "portal",
                table: "raid_loot",
                column: "loot_id",
                principalSchema: "portal",
                principalTable: "loot_item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_raid_loot_loot_item_loot_id",
                schema: "portal",
                table: "raid_loot");

            migrationBuilder.DropTable(
                name: "loot_item",
                schema: "portal");

            migrationBuilder.CreateTable(
                name: "boss_loot",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_description = table.Column<string>(type: "text", nullable: false),
                    grade_type = table.Column<int>(type: "integer", nullable: false),
                    item_name = table.Column<string>(type: "text", nullable: false),
                    synthesis_exp = table.Column<int>(type: "integer", nullable: true),
                    loot_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_boss_loot", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_raid_loot_boss_loot_loot_id",
                schema: "portal",
                table: "raid_loot",
                column: "loot_id",
                principalSchema: "portal",
                principalTable: "boss_loot",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
