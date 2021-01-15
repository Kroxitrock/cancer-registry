using Microsoft.EntityFrameworkCore.Migrations;

namespace CancerRegistry.Migrations
{
    public partial class AddedBustat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospitalBulstat",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospitalBulstat",
                table: "AspNetUsers");
        }
    }
}
