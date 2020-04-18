using Microsoft.EntityFrameworkCore.Migrations;

namespace Libraryhub.Data.Migrations
{
    public partial class ModifiedAlot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookPenalties");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookActivities");

            migrationBuilder.AddColumn<string>(
                name: "AdminUserId",
                table: "BookPenalties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "BookPenalties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminUserId",
                table: "BookActivities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "BookActivities",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "BookPenalties");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "BookPenalties");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "BookActivities");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "BookActivities");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookPenalties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
