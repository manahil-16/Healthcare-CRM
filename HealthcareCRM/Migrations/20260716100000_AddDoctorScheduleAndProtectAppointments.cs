using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthcareCRM.Migrations;

public partial class AddDoctorScheduleAndProtectAppointments : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "ScheduleDays",
            table: "Doctors",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.DropForeignKey(name: "FK_Appointments_Doctors_DoctorId", table: "Appointments");
        migrationBuilder.DropForeignKey(name: "FK_Appointments_Patients_PatientId", table: "Appointments");
        migrationBuilder.AddForeignKey(name: "FK_Appointments_Doctors_DoctorId", table: "Appointments", column: "DoctorId", principalTable: "Doctors", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
        migrationBuilder.AddForeignKey(name: "FK_Appointments_Patients_PatientId", table: "Appointments", column: "PatientId", principalTable: "Patients", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(name: "FK_Appointments_Doctors_DoctorId", table: "Appointments");
        migrationBuilder.DropForeignKey(name: "FK_Appointments_Patients_PatientId", table: "Appointments");
        migrationBuilder.DropColumn(name: "ScheduleDays", table: "Doctors");
        migrationBuilder.AddForeignKey(name: "FK_Appointments_Doctors_DoctorId", table: "Appointments", column: "DoctorId", principalTable: "Doctors", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        migrationBuilder.AddForeignKey(name: "FK_Appointments_Patients_PatientId", table: "Appointments", column: "PatientId", principalTable: "Patients", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
    }
}
