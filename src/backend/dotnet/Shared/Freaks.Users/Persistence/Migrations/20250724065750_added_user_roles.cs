using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_user_roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "roles",
                schema: "users",
                table: "users",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "roles",
                schema: "users",
                table: "users");
        }
    }
}
