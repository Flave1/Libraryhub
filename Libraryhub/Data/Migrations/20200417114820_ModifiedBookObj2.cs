using Microsoft.EntityFrameworkCore.Migrations;

namespace Libraryhub.Data.Migrations
{
    public partial class ModifiedBookObj2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessionNo",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassificationNo",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessionNo",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ClassificationNo",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Books");
        }
    }
}
