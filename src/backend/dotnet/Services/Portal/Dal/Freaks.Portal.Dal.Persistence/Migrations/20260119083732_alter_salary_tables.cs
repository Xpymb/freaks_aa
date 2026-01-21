using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class alter_salary_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Truncate all salary tables to avoid conflicts
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary_expenses CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary_guild_leader CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary_loot CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary_member CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary_parameters CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary CASCADE;");

            migrationBuilder.DropTable(
                name: "salary_parameters",
                schema: "portal");

            migrationBuilder.RenameColumn(
                name: "price_per_loot",
                schema: "portal",
                table: "salary_loot",
                newName: "price_per_item");

            migrationBuilder.RenameColumn(
                name: "price_per_loot",
                schema: "portal",
                table: "salary_guild_leader",
                newName: "price_per_item");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                schema: "portal",
                table: "salary_expenses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "portal",
                table: "salary_expenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "allowed_payment_types",
                schema: "portal",
                table: "salary",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<int[]>(
                name: "boss_types",
                schema: "portal",
                table: "salary",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<bool>(
                name: "use_coefficients",
                schema: "portal",
                table: "salary",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                schema: "portal",
                table: "salary_expenses");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "portal",
                table: "salary_expenses");

            migrationBuilder.DropColumn(
                name: "allowed_payment_types",
                schema: "portal",
                table: "salary");

            migrationBuilder.DropColumn(
                name: "boss_types",
                schema: "portal",
                table: "salary");

            migrationBuilder.DropColumn(
                name: "use_coefficients",
                schema: "portal",
                table: "salary");

            migrationBuilder.RenameColumn(
                name: "price_per_item",
                schema: "portal",
                table: "salary_loot",
                newName: "price_per_loot");

            migrationBuilder.RenameColumn(
                name: "price_per_item",
                schema: "portal",
                table: "salary_guild_leader",
                newName: "price_per_loot");

            migrationBuilder.CreateTable(
                name: "salary_parameters",
                schema: "portal",
                columns: table => new
                {
                    salary_id = table.Column<long>(type: "bigint", nullable: false),
                    allowed_payment_types = table.Column<int[]>(type: "integer[]", nullable: false),
                    boss_types = table.Column<int[]>(type: "integer[]", nullable: false),
                    use_coefficients = table.Column<bool>(type: "boolean", nullable: false)
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
        }
    }
}
