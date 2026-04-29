using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naloga1_Dinamicna.Migrations
{
    /// <inheritdoc />
    public partial class InicialnaBaza : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Izdelki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    Cena = table.Column<double>(type: "float", nullable: false),
                    DatumDobave = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izdelki", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uporabniki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priimek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumRojstva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Starost = table.Column<int>(type: "int", nullable: false),
                    Emso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Naslov = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostnaStevilka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Posta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Drzava = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uporabniki", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Izdelki");

            migrationBuilder.DropTable(
                name: "Uporabniki");
        }
    }
}
