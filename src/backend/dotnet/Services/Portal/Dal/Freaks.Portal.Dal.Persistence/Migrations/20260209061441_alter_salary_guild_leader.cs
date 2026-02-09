using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class alter_salary_guild_leader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary_expenses CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary_guild_leader CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary_loot CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary_member CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE portal.salary CASCADE;");

            migrationBuilder.DropForeignKey(
                name: "FK_salary_guild_leader_loot_item_loot_id",
                schema: "portal",
                table: "salary_guild_leader");

            migrationBuilder.DropIndex(
                name: "IX_salary_guild_leader_loot_id",
                schema: "portal",
                table: "salary_guild_leader");

            migrationBuilder.DropColumn(
                name: "amount",
                schema: "portal",
                table: "salary_guild_leader");

            migrationBuilder.DropColumn(
                name: "loot_id",
                schema: "portal",
                table: "salary_guild_leader");

            migrationBuilder.DropColumn(
                name: "price_per_item",
                schema: "portal",
                table: "salary_guild_leader");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "portal",
                table: "salary_guild_leader",
                newName: "salary_loot_id");

            migrationBuilder.AlterColumn<long>(
                name: "salary_loot_id",
                schema: "portal",
                table: "salary_guild_leader",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_salary_guild_leader_salary_loot_salary_loot_id",
                schema: "portal",
                table: "salary_guild_leader",
                column: "salary_loot_id",
                principalSchema: "portal",
                principalTable: "salary_loot",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salary_guild_leader_salary_loot_salary_loot_id",
                schema: "portal",
                table: "salary_guild_leader");

            migrationBuilder.RenameColumn(
                name: "salary_loot_id",
                schema: "portal",
                table: "salary_guild_leader",
                newName: "id");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "portal",
                table: "salary_guild_leader",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "amount",
                schema: "portal",
                table: "salary_guild_leader",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "loot_id",
                schema: "portal",
                table: "salary_guild_leader",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "price_per_item",
                schema: "portal",
                table: "salary_guild_leader",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_salary_guild_leader_loot_id",
                schema: "portal",
                table: "salary_guild_leader",
                column: "loot_id");

            migrationBuilder.AddForeignKey(
                name: "FK_salary_guild_leader_loot_item_loot_id",
                schema: "portal",
                table: "salary_guild_leader",
                column: "loot_id",
                principalSchema: "portal",
                principalTable: "loot_item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
