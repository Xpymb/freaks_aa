using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_shop_item_requests_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantity",
                schema: "portal",
                table: "shop_items",
                newName: "total_quantity");

            migrationBuilder.AddColumn<int>(
                name: "remaining_quantity",
                schema: "portal",
                table: "shop_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "shop_item_requests",
                schema: "portal",
                columns: table => new
                {
                    shop_item_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    created_dt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_item_requests", x => new { x.shop_item_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_shop_item_requests_shop_items_shop_item_id",
                        column: x => x.shop_item_id,
                        principalSchema: "portal",
                        principalTable: "shop_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shop_item_requests_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shop_item_requests_user_id",
                schema: "portal",
                table: "shop_item_requests",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shop_item_requests",
                schema: "portal");

            migrationBuilder.DropColumn(
                name: "remaining_quantity",
                schema: "portal",
                table: "shop_items");

            migrationBuilder.RenameColumn(
                name: "total_quantity",
                schema: "portal",
                table: "shop_items",
                newName: "quantity");
        }
    }
}
