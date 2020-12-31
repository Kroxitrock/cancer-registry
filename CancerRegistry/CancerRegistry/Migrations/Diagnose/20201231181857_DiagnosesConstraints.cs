using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CancerRegistry.Migrations.Diagnose
{
    public partial class DiagnosesConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistantMetastasis",
                table: "Diagnoses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrimaryTumor",
                table: "Diagnoses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegionalLymphNodes",
                table: "Diagnoses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HealthChecks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiagnoseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthChecks_Diagnoses_DiagnoseId",
                        column: x => x.DiagnoseId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Beginning = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Surgery = table.Column<int>(type: "int", nullable: false),
                    Radiation = table.Column<int>(type: "int", nullable: false),
                    Chemeotherapy = table.Column<int>(type: "int", nullable: false),
                    DiagnoseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treatments_Diagnoses_DiagnoseId",
                        column: x => x.DiagnoseId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_DiagnoseId",
                table: "HealthChecks",
                column: "DiagnoseId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DiagnoseId",
                table: "Treatments",
                column: "DiagnoseId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthChecks");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropColumn(
                name: "DistantMetastasis",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "PrimaryTumor",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "RegionalLymphNodes",
                table: "Diagnoses");
        }
    }
}
