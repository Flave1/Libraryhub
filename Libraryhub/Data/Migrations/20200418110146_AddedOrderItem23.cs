using Microsoft.EntityFrameworkCore.Migrations;

namespace Libraryhub.Data.Migrations
{
    public partial class AddedOrderItem23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantitySold",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantitySold",
                table: "Books");
        }
    }
}
