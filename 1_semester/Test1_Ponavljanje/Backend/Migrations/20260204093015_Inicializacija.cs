using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class Inicializacija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nabiralniki",
                columns: table => new
                {
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                  //  ,Aktiven = table.Column<string>(type:"Text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nabiralniki", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Sporocila",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Zadeva = table.Column<string>(type: "TEXT", nullable: false),
                    Vsebina = table.Column<string>(type: "TEXT", nullable: false),
                    CasPosiljanja = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    PrejemnikEmail = table.Column<string>(type: "TEXT", nullable: false),
                    PosiljateljEmail = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sporocila", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sporocila_Nabiralniki_PosiljateljEmail",
                        column: x => x.PosiljateljEmail,
                        principalTable: "Nabiralniki",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sporocila_Nabiralniki_PrejemnikEmail",
                        column: x => x.PrejemnikEmail,
                        principalTable: "Nabiralniki",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sporocila_PosiljateljEmail",
                table: "Sporocila",
                column: "PosiljateljEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Sporocila_PrejemnikEmail",
                table: "Sporocila",
                column: "PrejemnikEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sporocila");

            migrationBuilder.DropTable(
                name: "Nabiralniki");
        }
    }
}
