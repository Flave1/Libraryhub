using Microsoft.EntityFrameworkCore.Migrations;

namespace Libraryhub.Data.Migrations
{
    public partial class ModifiedBookObjAddedInitialQty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitialQuantity",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialQuantity",
                table: "Books");
        }
    }
}
