using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_salary_final_report_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_finished",
                schema: "portal",
                table: "salary",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "salary_final_report",
                schema: "portal",
                columns: table => new
                {
                    salary_id = table.Column<long>(type: "bigint", nullable: false),
                    total_gold = table.Column<decimal>(type: "numeric", nullable: false),
                    total_world_boss_infusion = table.Column<int>(type: "integer", nullable: false),
                    total_erenor_infusion = table.Column<int>(type: "integer", nullable: false),
                    gold_guild_leader_expenses = table.Column<decimal>(type: "numeric", nullable: false),
                    world_boss_infusion_guild_leader_expenses = table.Column<int>(type: "integer", nullable: false),
                    erenor_infusion_guild_leader_expenses = table.Column<int>(type: "integer", nullable: false),
                    gold_expenses = table.Column<decimal>(type: "numeric", nullable: false),
                    world_boss_infusion_expenses = table.Column<decimal>(type: "numeric", nullable: false),
                    erenor_infusion_expenses = table.Column<decimal>(type: "numeric", nullable: false),
                    gold_for_salary = table.Column<decimal>(type: "numeric", nullable: false),
                    world_boss_infusion_for_salary = table.Column<decimal>(type: "numeric", nullable: false),
                    world_boss_infusion_for_sale = table.Column<decimal>(type: "numeric", nullable: false),
                    erenor_infusion_for_salary = table.Column<decimal>(type: "numeric", nullable: false),
                    erenor_infusion_for_sale = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salary_final_report", x => x.salary_id);
                    table.ForeignKey(
                        name: "FK_salary_final_report_salary_salary_id",
                        column: x => x.salary_id,
                        principalSchema: "portal",
                        principalTable: "salary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salary_final_report",
                schema: "portal");

            migrationBuilder.DropColumn(
                name: "is_finished",
                schema: "portal",
                table: "salary");
        }
    }
}
