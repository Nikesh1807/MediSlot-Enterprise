using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthcareApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDoctorsAndSlotsUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "Name", "Specialization" },
                values: new object[,]
                {
                    { 1, "Dr. Sharma", "General Physician" },
                    { 2, "Dr. Priya Patel", "Cardiologist" },
                    { 3, "Dr. Rajesh Mehta", "Dermatologist" },
                    { 4, "Dr. Ananya Roy", "Pediatrician" },
                    { 5, "Dr. Vikram Joshi", "Orthopedic Surgeon" },
                    { 6, "Dr. Neha Gupta", "Neurologist" }
                });

            migrationBuilder.InsertData(
                table: "TimeSlots",
                columns: new[] { "Id", "DoctorId", "EndTime", "IsBooked", "IsLocked", "LockExpiresAt", "StartTime" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 11, 25, 9, 30, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 1, new DateTime(2026, 11, 25, 10, 0, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 9, 30, 0, 0, DateTimeKind.Utc) },
                    { 3, 1, new DateTime(2026, 11, 25, 10, 30, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 2, new DateTime(2026, 11, 25, 11, 30, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 2, new DateTime(2026, 11, 25, 12, 0, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 11, 30, 0, 0, DateTimeKind.Utc) },
                    { 6, 3, new DateTime(2026, 11, 25, 14, 30, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, 3, new DateTime(2026, 11, 25, 15, 0, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 14, 30, 0, 0, DateTimeKind.Utc) },
                    { 8, 4, new DateTime(2026, 11, 25, 16, 30, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 4, new DateTime(2026, 11, 25, 17, 0, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 16, 30, 0, 0, DateTimeKind.Utc) },
                    { 10, 5, new DateTime(2026, 11, 25, 10, 30, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, 5, new DateTime(2026, 11, 25, 11, 0, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 10, 30, 0, 0, DateTimeKind.Utc) },
                    { 12, 6, new DateTime(2026, 11, 25, 17, 30, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 17, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, 6, new DateTime(2026, 11, 25, 18, 0, 0, 0, DateTimeKind.Utc), false, false, null, new DateTime(2026, 11, 25, 17, 30, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
