using static CCC.Tranzakcija;

namespace CCC
{
    public static class TranzakcijaEndPont
    {
        public static void TranzakcijaIzpis(this WebApplication app)
        {
            app.MapPost("/api/transakcija/posodobi-oboje",   async (PodatkiPB db, PosodobitevStudentaInNalogeDto zahteva) =>
            {
                // Uporaba 'await using' poskrbi, da se transakcija pravilno zapre.
                await using var transakcija = await db.Database.BeginTransactionAsync();

                try
                {
                    var posStudent = await db.VsiStudentje.FindAsync(zahteva.StudentId);
                    if (posStudent == null)
                    {
                        return Results.NotFound("Nema Majstra z Idem");
                    }

                    if (zahteva.NovaStarost < 5)
                    {
                        throw new ArgumentException("Starost je prenizka in bo sprožila Rollback.");
                    }

                    posStudent.age = zahteva.NovaStarost;

                    await db.SaveChangesAsync();

                    var naloga = await db.VseNaloge.FindAsync(zahteva.NalogaId);

                    if (naloga == null)
                    {
                        // Če naloge ne najdemo, vrnemo napako, ki bo prekinila to 'try' vejo.
                        return Results.NotFound($"Naloga z ID {zahteva.NalogaId} ni najdena.");
                    }

                    naloga.JeKoncana = zahteva.NovoStanjeNaloge;
                    await db.SaveChangesAsync();


                    await transakcija.CommitAsync();


                    return Results.Ok(new
                    {
                        Status = "Uspešno",
                        Sporocilo = "Transakcija uspešna. Obe tabeli sta posodobljeni."
                    });

                }
                catch (Exception ex)
                {
                    await transakcija.RollbackAsync();

                    return Results.Json(new
                    {
                        Status = "Neuspešno",
                        Sporocilo = "Transakcija razveljavljena (Rollback). Nobena sprememba ni shranjena.",
                        Napaka = ex.Message
                    }, statusCode: 500);
                }
            
            })  .WithTags("01 - Transakcije")
                .WithSummary("Posodobi hkrati tabeli Študent in Naloga z uporabo zunanje transakcije.")
                .Produces(200, typeof(object)) // Uspešen commit (Vrača status objekt)
                .Produces(500, typeof(object)); ;
        }
    
    }
}
