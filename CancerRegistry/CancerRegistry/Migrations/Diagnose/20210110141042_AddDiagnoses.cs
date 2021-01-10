using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CancerRegistry.Migrations.Diagnose
{
    public partial class AddDiagnoses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence(
                name: "Diagnoses_seq",
                schema: "dbo");

            migrationBuilder.CreateSequence(
                name: "HealthChecks_seq",
                schema: "dbo");

            migrationBuilder.CreateSequence(
                name: "Treatments_seq",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    ActiveDiagnoseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "NEXT VALUE FOR dbo.Diagnoses_seq"),
                    DoctorUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Stage = table.Column<short>(type: "smallint", nullable: false),
                    PrimaryTumor = table.Column<int>(type: "int", nullable: false),
                    DistantMetastasis = table.Column<int>(type: "int", nullable: false),
                    RegionalLymphNodes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Doctors_DoctorUserId",
                        column: x => x.DoctorUserId,
                        principalTable: "Doctors",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Patients_PatientUserId",
                        column: x => x.PatientUserId,
                        principalTable: "Patients",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthChecks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "NEXT VALUE FOR dbo.HealthChecks_seq"),
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
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "NEXT VALUE FOR dbo.Treatments_seq"),
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
                name: "IX_Diagnoses_DoctorUserId",
                table: "Diagnoses",
                column: "DoctorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_PatientUserId",
                table: "Diagnoses",
                column: "PatientUserId");

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

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropSequence(
                name: "Diagnoses_seq",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "HealthChecks_seq",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "Treatments_seq",
                schema: "dbo");
        }
    }
}
