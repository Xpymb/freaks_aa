using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class alter_salary_expenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_salary_expenses",
                schema: "portal",
                table: "salary_expenses");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "portal",
                table: "salary_expenses",
                newName: "id");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "portal",
                table: "salary_expenses",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_salary_expenses",
                schema: "portal",
                table: "salary_expenses",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_salary_expenses_salary_id",
                schema: "portal",
                table: "salary_expenses",
                column: "salary_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_salary_expenses",
                schema: "portal",
                table: "salary_expenses");

            migrationBuilder.DropIndex(
                name: "IX_salary_expenses_salary_id",
                schema: "portal",
                table: "salary_expenses");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "portal",
                table: "salary_expenses",
                newName: "Id");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "portal",
                table: "salary_expenses",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_salary_expenses",
                schema: "portal",
                table: "salary_expenses",
                columns: new[] { "salary_id", "expenses_type" });
        }
    }
}
