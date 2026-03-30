using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vaja4NalogaTest.Migrations
{
    /// <inheritdoc />
    public partial class AddPublisher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZaloznikId",
                table: "VseKnjige",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VsiZaložniki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsiZaložniki", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VseKnjige_ZaloznikId",
                table: "VseKnjige",
                column: "ZaloznikId");

            migrationBuilder.AddForeignKey(
                name: "FK_VseKnjige_VsiZaložniki_ZaloznikId",
                table: "VseKnjige",
                column: "ZaloznikId",
                principalTable: "VsiZaložniki",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VseKnjige_VsiZaložniki_ZaloznikId",
                table: "VseKnjige");

            migrationBuilder.DropTable(
                name: "VsiZaložniki");

            migrationBuilder.DropIndex(
                name: "IX_VseKnjige_ZaloznikId",
                table: "VseKnjige");

            migrationBuilder.DropColumn(
                name: "ZaloznikId",
                table: "VseKnjige");
        }
    }
}
