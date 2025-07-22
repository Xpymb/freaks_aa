using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "portal");

            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.CreateTable(
                name: "boss_loot",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    loot_type = table.Column<int>(type: "integer", nullable: false),
                    grade_type = table.Column<int>(type: "integer", nullable: false),
                    item_name = table.Column<string>(type: "text", nullable: false),
                    synthesis_exp = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_boss_loot", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "raid",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    boss_type = table.Column<int>(type: "integer", nullable: false),
                    format_type = table.Column<int>(type: "integer", nullable: true),
                    start_dt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_dt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_dt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raid", x => x.id);
                    table.ForeignKey(
                        name: "FK_raid_users_creator_id",
                        column: x => x.creator_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "raid_loot",
                schema: "portal",
                columns: table => new
                {
                    raid_id = table.Column<int>(type: "integer", nullable: false),
                    loot_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raid_loot", x => new { x.raid_id, x.loot_id });
                    table.ForeignKey(
                        name: "FK_raid_loot_boss_loot_loot_id",
                        column: x => x.loot_id,
                        principalSchema: "portal",
                        principalTable: "boss_loot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_raid_loot_raid_raid_id",
                        column: x => x.raid_id,
                        principalSchema: "portal",
                        principalTable: "raid",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "raid_participant",
                schema: "portal",
                columns: table => new
                {
                    raid_id = table.Column<int>(type: "integer", nullable: false),
                    participant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raid_participant", x => new { x.raid_id, x.participant_id });
                    table.ForeignKey(
                        name: "FK_raid_participant_raid_raid_id",
                        column: x => x.raid_id,
                        principalSchema: "portal",
                        principalTable: "raid",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_raid_participant_users_participant_id",
                        column: x => x.participant_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "raid_screenshot",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    screenshot_url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raid_screenshot", x => new { x.id, x.screenshot_url });
                    table.ForeignKey(
                        name: "FK_raid_screenshot_raid_id",
                        column: x => x.id,
                        principalSchema: "portal",
                        principalTable: "raid",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_raid_creator_id",
                schema: "portal",
                table: "raid",
                column: "creator_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_raid_loot_loot_id",
                schema: "portal",
                table: "raid_loot",
                column: "loot_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_raid_participant_participant_id",
                schema: "portal",
                table: "raid_participant",
                column: "participant_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "raid_loot",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "raid_participant",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "raid_screenshot",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "boss_loot",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "raid",
                schema: "portal");
        }
    }
}
