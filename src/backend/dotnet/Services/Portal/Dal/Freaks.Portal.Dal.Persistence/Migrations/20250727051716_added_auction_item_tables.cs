using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_auction_item_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auction_item",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    loot_item_id = table.Column<int>(type: "integer", nullable: false),
                    start_price = table.Column<decimal>(type: "numeric", nullable: false),
                    min_price_step = table.Column<decimal>(type: "numeric", nullable: false),
                    created_dt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_dt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auction_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_auction_item_loot_item_loot_item_id",
                        column: x => x.loot_item_id,
                        principalSchema: "portal",
                        principalTable: "loot_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_auction_item_users_creator_id",
                        column: x => x.creator_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auction_item_bid",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    auction_item_id = table.Column<long>(type: "bigint", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    created_dt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auction_item_bid", x => x.id);
                    table.ForeignKey(
                        name: "FK_auction_item_bid_auction_item_auction_item_id",
                        column: x => x.auction_item_id,
                        principalSchema: "portal",
                        principalTable: "auction_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_auction_item_bid_users_creator_id",
                        column: x => x.creator_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_auction_item_creator_id",
                schema: "portal",
                table: "auction_item",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_item_loot_item_id",
                schema: "portal",
                table: "auction_item",
                column: "loot_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_item_bid_auction_item_id",
                schema: "portal",
                table: "auction_item_bid",
                column: "auction_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_item_bid_creator_id",
                schema: "portal",
                table: "auction_item_bid",
                column: "creator_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auction_item_bid",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "auction_item",
                schema: "portal");
        }
    }
}
