using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevorked",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "JwtId",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "RefreshTokens",
                newName: "Deactivated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deactivated",
                table: "RefreshTokens",
                newName: "IsUsed");

            migrationBuilder.AddColumn<bool>(
                name: "IsRevorked",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JwtId",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
