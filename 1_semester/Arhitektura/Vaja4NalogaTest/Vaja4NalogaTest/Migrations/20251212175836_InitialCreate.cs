using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vaja4NalogaTest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VseKnjige",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naslov = table.Column<string>(type: "TEXT", nullable: false),
                    Avtor = table.Column<string>(type: "TEXT", nullable: false),
                    DatumIzdelave = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VseKnjige", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VseKnjige");
        }
    }
}
