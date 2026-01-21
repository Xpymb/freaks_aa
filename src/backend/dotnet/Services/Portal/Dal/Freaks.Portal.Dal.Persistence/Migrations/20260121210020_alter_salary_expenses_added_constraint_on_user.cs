using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class alter_salary_expenses_added_constraint_on_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_salary_expenses_user_id",
                schema: "portal",
                table: "salary_expenses",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_salary_expenses_users_user_id",
                schema: "portal",
                table: "salary_expenses",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salary_expenses_users_user_id",
                schema: "portal",
                table: "salary_expenses");

            migrationBuilder.DropIndex(
                name: "IX_salary_expenses_user_id",
                schema: "portal",
                table: "salary_expenses");
        }
    }
}
