using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStore.Services.Migrations
{
    public partial class addUpdateByInventoryHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "InventoryHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "InventoryHistory");
        }
    }
}
