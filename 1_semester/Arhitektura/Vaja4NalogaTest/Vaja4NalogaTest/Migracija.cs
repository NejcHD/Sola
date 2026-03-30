using Microsoft.EntityFrameworkCore.Migrations;

namespace Vaja4NalogaTest
{
    // POMEMBNO: Ime razreda mora ustrezati imenu migracije
    public partial class AddPublisher : Migration
    {
        // ====================================================================
        // METODA UP: Posodablja bazo (Izvede spremembe)
        // ====================================================================
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. USTVARJANJE NOVE TABELE (Založnik)
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

            // 2. DODAJANJE TUJEGA KLJUČA (ZaloznikId) v tabelo Knjiga
            migrationBuilder.AddColumn<int>(
                name: "ZaloznikId",
                table: "VseKnjige",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            // 3. DODAJANJE OMEJITVE TUJEGA KLJUČA (Foreign Key Constraint)
            // Za SQLite uporabi ReferentialAction.NoAction ali ReferentialAction.Restrict
            migrationBuilder.AddForeignKey(
                name: "FK_VseKnjige_VsiZaložniki_ZaloznikId",
                table: "VseKnjige",
                column: "ZaloznikId",
                principalTable: "VsiZaložniki",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);  // SPREMENI!
        }

        // ====================================================================
        // METODA DOWN: Razveljavi spremembe (Vrne bazo v prejšnje stanje)
        // ====================================================================
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Odstranitev omejitve tujega ključa
            migrationBuilder.DropForeignKey(
                name: "FK_VseKnjige_VsiZaložniki_ZaloznikId",
                table: "VseKnjige");

            // 2. Odstranitev stolpca tujega ključa iz tabele Knjiga
            migrationBuilder.DropColumn(
                name: "ZaloznikId",
                table: "VseKnjige");

            // 3. Brisanje tabele Založnik
            migrationBuilder.DropTable(
                name: "VsiZaložniki");
        }
    }
}