using Microsoft.EntityFrameworkCore;
using Projekt;
using Projekt.Models;

namespace TriAtlon_Nal2.EndPoints
{
    public static class RezultatiEndpoints
    {
        public static void MapRezultatiEndpoints(this IEndpointRouteBuilder app)
        {
            // PRIDOBI VSE REZULTATE
            app.MapGet("/api/rezultati", async (ApplicationDbContext db) =>
            {
                var seznam = await db.Rezultati.ToListAsync();
                return Results.Ok(seznam);
            })
            .WithTags("Rezultati")
            .WithSummary("Pridobi seznam vseh rezultatev iz baze.");

            app.MapGet("/api/rezultati/tekmovalci", async (ApplicationDbContext db) =>
            {
                var seznam = await db.Rezultati
                    .Include(r => r.Tekmovalci) // doda tekmovalca
                    .Take(100)
                    .ToListAsync();
                return Results.Ok(seznam);
            })
             .WithTags("Rezultati")
             .WithSummary("Pridobi seznam tekmovalcev ter rezultatov iz baze.");

            // DODAJ NOV REZULTAT
            app.MapPost("/api/rezultati", async (Rezultati novi, ApplicationDbContext db) =>
            {
                db.Rezultati.Add(novi);
                await db.SaveChangesAsync();
                return Results.Created($"/api/rezultati/{novi.idRezultata}", novi);
            })
            .WithTags("Rezultati")
            .WithSummary("Doda rezultat.");

            // IZBRIŠI REZULTAT
            app.MapDelete("/api/rezultati/{id}", async (int id, ApplicationDbContext db) =>
            {
                var rezultat = await db.Rezultati.FindAsync(id);
                if (rezultat == null) return Results.NotFound();

                db.Rezultati.Remove(rezultat);
                await db.SaveChangesAsync();
                return Results.Ok(rezultat);
            })
             .WithTags("Rezultati")
             .WithSummary("Izbrisi rezultat po id-u.");
     


            app.MapGet("/api/rezultati/iskanje/{priimek}", async (string priimek, ApplicationDbContext db) =>
            {
                var rezultati = await db.Rezultati
                    .Include(r => r.Tekmovalci)
                    .Include(r => r.Tekma)
                    .Where(r => r.Tekmovalci.Ime_Priimek.Contains(priimek))
                    .ToListAsync();

                return Results.Ok(rezultati);
            })
            .WithTags("Rezultati")
            .WithSummary("Poišče vse rezultate tekmovalca po delu njegovega priimku.");


            app.MapPut("/api/rezultati/{id}", async (int id, Rezultati posodobljen, ApplicationDbContext db) =>
            {
                var obstojeci = await db.Rezultati.FindAsync(id);
                if (obstojeci is null) return Results.NotFound("Rezultat s tem ID ne obstaja.");

                
                obstojeci.Cas_Plavanja = posodobljen.Cas_Plavanja;
                obstojeci.Cas_Kolesarjenja = posodobljen.Cas_Kolesarjenja;
                obstojeci.Cas_Teka = posodobljen.Cas_Teka;
                obstojeci.Skupni_cas = posodobljen.Skupni_cas;
                obstojeci.Uvrstitev = posodobljen.Uvrstitev;


                if (posodobljen.Tekma_idTekma > 0)                              // lahko izpustis v swagerju
                    obstojeci.Tekma_idTekma = posodobljen.Tekma_idTekma;

                if (posodobljen.Tekmovalci_idTekmovalci > 0)
                    obstojeci.Tekmovalci_idTekmovalci = posodobljen.Tekmovalci_idTekmovalci;

                await db.SaveChangesAsync();
                return Results.NoContent(); 
            })
            .WithTags("Rezultati")
            .WithSummary("Posodobi podatke o rezultatu.");
        }
    }
}