using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIS_16_10_3.Migrations
{
    /// <inheritdoc />
    public partial class Kosarica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KosaricaId",
                table: "artikli",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "kosarice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kosarice", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_artikli_KosaricaId",
                table: "artikli",
                column: "KosaricaId");

            migrationBuilder.AddForeignKey(
                name: "FK_artikli_kosarice_KosaricaId",
                table: "artikli",
                column: "KosaricaId",
                principalTable: "kosarice",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_artikli_kosarice_KosaricaId",
                table: "artikli");

            migrationBuilder.DropTable(
                name: "kosarice");

            migrationBuilder.DropIndex(
                name: "IX_artikli_KosaricaId",
                table: "artikli");

            migrationBuilder.DropColumn(
                name: "KosaricaId",
                table: "artikli");
        }
    }
}
