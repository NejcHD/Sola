using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vaja4NalogaTest.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VsiZaložniki",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Založba Modrijan" },
                    { 2, "Mladinska Knjiga" }
                });

            migrationBuilder.InsertData(
                table: "VseKnjige",
                columns: new[] { "Id", "Avtor", "DatumIzdelave", "Naslov", "ZaloznikId" },
                values: new object[] { 1, "Jules Verne", new DateTime(2025, 12, 15, 13, 6, 57, 672, DateTimeKind.Local).AddTicks(2749), "Potovanje na luno", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VseKnjige",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VsiZaložniki",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VsiZaložniki",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
