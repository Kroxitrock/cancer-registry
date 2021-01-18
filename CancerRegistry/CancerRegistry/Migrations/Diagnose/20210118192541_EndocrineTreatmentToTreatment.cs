using Microsoft.EntityFrameworkCore.Migrations;

namespace CancerRegistry.Migrations.Diagnose
{
    public partial class EndocrineTreatmentToTreatment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndocrineTreatment",
                table: "Treatments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndocrineTreatment",
                table: "Treatments");
        }
    }
}
