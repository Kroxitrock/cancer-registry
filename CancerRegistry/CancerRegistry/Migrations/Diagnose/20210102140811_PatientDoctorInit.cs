using Microsoft.EntityFrameworkCore.Migrations;

namespace CancerRegistry.Migrations.Diagnose
{
    public partial class PatientDoctorInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorUserId",
                table: "Diagnoses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientUserId",
                table: "Diagnoses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EGN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiplomaNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EIK = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    ActiveDiagnoseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_DoctorUserId",
                table: "Diagnoses",
                column: "DoctorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_PatientUserId",
                table: "Diagnoses",
                column: "PatientUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Doctors_DoctorUserId",
                table: "Diagnoses",
                column: "DoctorUserId",
                principalTable: "Doctors",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Patients_PatientUserId",
                table: "Diagnoses",
                column: "PatientUserId",
                principalTable: "Patients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Doctors_DoctorUserId",
                table: "Diagnoses");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Patients_PatientUserId",
                table: "Diagnoses");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_DoctorUserId",
                table: "Diagnoses");

            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_PatientUserId",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "DoctorUserId",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "PatientUserId",
                table: "Diagnoses");
        }
    }
}
