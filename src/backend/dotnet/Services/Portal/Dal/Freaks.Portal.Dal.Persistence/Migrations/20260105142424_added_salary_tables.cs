using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_salary_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "salary",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    start_dt = table.Column<DateOnly>(type: "date", nullable: false),
                    end_dt = table.Column<DateOnly>(type: "date", nullable: false),
                    fill_status = table.Column<int>(type: "integer", nullable: false),
                    registration_status = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_dt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_dt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salary", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "salary_expenses",
                schema: "portal",
                columns: table => new
                {
                    salary_id = table.Column<long>(type: "bigint", nullable: false),
                    expenses_type = table.Column<int>(type: "integer", nullable: false),
                    percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salary_expenses", x => new { x.salary_id, x.expenses_type });
                    table.ForeignKey(
                        name: "FK_salary_expenses_salary_salary_id",
                        column: x => x.salary_id,
                        principalSchema: "portal",
                        principalTable: "salary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salary_guild_leader",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    salary_id = table.Column<long>(type: "bigint", nullable: false),
                    loot_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price_per_loot = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salary_guild_leader", x => x.id);
                    table.ForeignKey(
                        name: "FK_salary_guild_leader_loot_item_loot_id",
                        column: x => x.loot_id,
                        principalSchema: "portal",
                        principalTable: "loot_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_salary_guild_leader_salary_salary_id",
                        column: x => x.salary_id,
                        principalSchema: "portal",
                        principalTable: "salary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salary_loot",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    salary_id = table.Column<long>(type: "bigint", nullable: false),
                    loot_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price_per_loot = table.Column<decimal>(type: "numeric", nullable: false),
                    discount_percent = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salary_loot", x => x.id);
                    table.ForeignKey(
                        name: "FK_salary_loot_loot_item_loot_id",
                        column: x => x.loot_id,
                        principalSchema: "portal",
                        principalTable: "loot_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_salary_loot_salary_salary_id",
                        column: x => x.salary_id,
                        principalSchema: "portal",
                        principalTable: "salary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salary_member",
                schema: "portal",
                columns: table => new
                {
                    salary_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_type = table.Column<int>(type: "integer", nullable: false),
                    activity_percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    coefficient = table.Column<decimal>(type: "numeric", nullable: true),
                    activity_gold = table.Column<decimal>(type: "numeric", nullable: true),
                    responsibility_gold = table.Column<decimal>(type: "numeric", nullable: true),
                    amount_gold = table.Column<decimal>(type: "numeric", nullable: true),
                    amount_world_boss_infusion = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salary_member", x => new { x.salary_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_salary_member_salary_salary_id",
                        column: x => x.salary_id,
                        principalSchema: "portal",
                        principalTable: "salary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_salary_member_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salary_parameters",
                schema: "portal",
                columns: table => new
                {
                    salary_id = table.Column<long>(type: "bigint", nullable: false),
                    allowed_payment_types = table.Column<int[]>(type: "integer[]", nullable: false),
                    use_coefficients = table.Column<bool>(type: "boolean", nullable: false),
                    boss_types = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salary_parameters", x => x.salary_id);
                    table.ForeignKey(
                        name: "FK_salary_parameters_salary_salary_id",
                        column: x => x.salary_id,
                        principalSchema: "portal",
                        principalTable: "salary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_salary_guild_leader_loot_id",
                schema: "portal",
                table: "salary_guild_leader",
                column: "loot_id");

            migrationBuilder.CreateIndex(
                name: "IX_salary_guild_leader_salary_id",
                schema: "portal",
                table: "salary_guild_leader",
                column: "salary_id");

            migrationBuilder.CreateIndex(
                name: "IX_salary_loot_loot_id",
                schema: "portal",
                table: "salary_loot",
                column: "loot_id");

            migrationBuilder.CreateIndex(
                name: "IX_salary_loot_salary_id",
                schema: "portal",
                table: "salary_loot",
                column: "salary_id");

            migrationBuilder.CreateIndex(
                name: "IX_salary_member_user_id",
                schema: "portal",
                table: "salary_member",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salary_expenses",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "salary_guild_leader",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "salary_loot",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "salary_member",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "salary_parameters",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "salary",
                schema: "portal");
        }
    }
}
