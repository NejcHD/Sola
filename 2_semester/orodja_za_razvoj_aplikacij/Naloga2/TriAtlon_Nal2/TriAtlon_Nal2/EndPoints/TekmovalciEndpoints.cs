using Microsoft.EntityFrameworkCore;
using Projekt;
using Projekt.Models;

namespace TriAtlon_Nal2.EndPoints
{
    public static class TekmovalciEndpoints
    {
        public static void MapTekmovalciEndpoints(this IEndpointRouteBuilder app)
        {
            // PRIDOBI VSE 
            app.MapGet("/api/tekmovalci", async (ApplicationDbContext db) =>
            {
                Console.WriteLine("--> Začenjam branje iz baze...");

                try
                {
                    var seznam = await db.Tekmovalci.ToListAsync();
                    Console.WriteLine($"--> Prejeto {seznam.Count} tekmovalcev.");
                    return Results.Ok(seznam);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> NAPAKA: {ex.Message}");
                    return Results.Problem(ex.Message);
                }
            })
            .WithTags("Tekmovalci")
            .WithSummary("Pridobi seznam vseh tekmovalcev iz baze."); 

            //  PRIDOBI ENEGA 
            app.MapGet("/api/tekmovalci/{id}", async (int id, ApplicationDbContext db) =>
            {

                var tekmovalec = await db.Tekmovalci.FindAsync(id);

                // Če ga ne najdemo, vrnemo 404
                if (tekmovalec == null)
                {
                    return Results.NotFound("Tekmovalca s tem ID-jem ni mogoče najti.");
                }


                return Results.Ok(tekmovalec);
            })
            .WithTags("Tekmovalci")
            .WithSummary("Pridobi tekmovalca po id-u iz baze.");

            app.MapGet("/api/tekmovalci/podrobno/{id}", async (int id, ApplicationDbContext db) =>
            {
                
                var tekmovalec = await db.Tekmovalci
                    .Include(t => t.Drzava)
                    .FirstOrDefaultAsync(t => t.idTekmovalci == id);

              
                if (tekmovalec == null)
                {
                    return Results.NotFound($"Tekmovalec z ID {id} ne obstaja.");
                }

                return Results.Ok(tekmovalec);
            })
            .WithTags("Tekmovalci")
            .WithSummary("Pridobi podrobne podatke o enem tekmovalcu z državo po njegovem ID.");

            //  DODAJ NOVEGA 
            app.MapPost("/api/tekmovalci", async (Tekmovalci noviTekmovalec, ApplicationDbContext db) =>
            {
                // Dodamo objekt v čakalnico baze
                db.Tekmovalci.Add(noviTekmovalec);


                await db.SaveChangesAsync();

               
                return Results.Created($"/api/tekmovalci/{noviTekmovalec.idTekmovalci}", noviTekmovalec);
            })
            .WithTags("Tekmovalci")
            .WithSummary("Doda tekmovalca");

            //  POSODOBI
            app.MapPut("/api/tekmovalci/{id}", async (int id, Tekmovalci posodobljeniPodatki, ApplicationDbContext db) =>
            {
                
                var tekmovalecIzBaze = await db.Tekmovalci.FindAsync(id);

                if (tekmovalecIzBaze == null)
                {
                    return Results.NotFound();
                }

              
                tekmovalecIzBaze.Ime_Priimek = posodobljeniPodatki.Ime_Priimek;
                tekmovalecIzBaze.Spol = posodobljeniPodatki.Spol;
                tekmovalecIzBaze.Drzava_idDrzava = posodobljeniPodatki.Drzava_idDrzava;


                await db.SaveChangesAsync();

                // Status 204 pomeni "Uspešno posodobljeno, nimam pa kaj novega za pokazat"
                return Results.NoContent();
            })
            .WithTags("Tekmovalci")
            .WithSummary("posodobi tekmovalca po id-u.");

            //  IZBRIŠI 
            app.MapDelete("/api/tekmovalci/{id}", async (int id, ApplicationDbContext db) =>
            {

                var tekmovalec = await db.Tekmovalci.FindAsync(id);

                if (tekmovalec == null)
                {
                    return Results.NotFound();
                }


                db.Tekmovalci.Remove(tekmovalec);


                await db.SaveChangesAsync();

                
                return Results.Ok(tekmovalec);
            })
            .WithTags("Tekmovalci")
            .WithSummary("Izbrisi tekmovalca po id-u."); 
        }
    }
}