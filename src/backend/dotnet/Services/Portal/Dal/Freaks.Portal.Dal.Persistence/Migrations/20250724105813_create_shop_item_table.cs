using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class create_shop_item_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_raid_loot_loot_item_loot_id",
                schema: "portal",
                table: "raid_loot");

            migrationBuilder.RenameColumn(
                name: "amount",
                schema: "portal",
                table: "raid_loot",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "loot_id",
                schema: "portal",
                table: "raid_loot",
                newName: "loot_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_raid_loot_loot_id",
                schema: "portal",
                table: "raid_loot",
                newName: "IX_raid_loot_loot_item_id");

            migrationBuilder.CreateTable(
                name: "shop_items",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    loot_item_id = table.Column<int>(type: "integer", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_shop_items_loot_item_loot_item_id",
                        column: x => x.loot_item_id,
                        principalSchema: "portal",
                        principalTable: "loot_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shop_items_users_creator_id",
                        column: x => x.creator_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shop_items_creator_id",
                schema: "portal",
                table: "shop_items",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_shop_items_loot_item_id",
                schema: "portal",
                table: "shop_items",
                column: "loot_item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_raid_loot_loot_item_loot_item_id",
                schema: "portal",
                table: "raid_loot",
                column: "loot_item_id",
                principalSchema: "portal",
                principalTable: "loot_item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_raid_loot_loot_item_loot_item_id",
                schema: "portal",
                table: "raid_loot");

            migrationBuilder.DropTable(
                name: "shop_items",
                schema: "portal");

            migrationBuilder.RenameColumn(
                name: "quantity",
                schema: "portal",
                table: "raid_loot",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "loot_item_id",
                schema: "portal",
                table: "raid_loot",
                newName: "loot_id");

            migrationBuilder.RenameIndex(
                name: "IX_raid_loot_loot_item_id",
                schema: "portal",
                table: "raid_loot",
                newName: "IX_raid_loot_loot_id");

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
    }
}
