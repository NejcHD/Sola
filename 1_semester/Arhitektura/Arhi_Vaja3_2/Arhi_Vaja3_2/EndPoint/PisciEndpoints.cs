using Arhi_Vaja3.Models;
using Arhi_Vaja3.Data;

namespace Arhi_Vaja3.Endpoints
{
    public static class PisciEndpoints  // ← MORDA je "PisciEndpoints"
    {
        public static void MapPisciEndpoints(this WebApplication app)  // ← TO JE PRAVILNO IMETJE
        {
            // GET /api/pisci - Pridobi vse avtorje
            app.MapGet("/api/pisci", () =>
            {
                return Results.Ok(DataContext.VsiAvtorji);
            })
            .WithName("GetVsiAvtorji");

            // GET /api/pisci/{id} - Pridobi posameznega avtorja
            app.MapGet("/api/pisci/{id}", (int id) =>
            {
                var pisec = DataContext.VsiAvtorji.FirstOrDefault(p => p.Id == id);
                if (pisec == null)
                {
                    return Results.NotFound($"Avtor z ID {id} ne obstaja.");
                }
                return Results.Ok(pisec);
            })
            .WithName("GetPisec");

            // POST /api/pisci - Dodaj novega avtorja
            app.MapPost("/api/pisci", (Pisec novPisec) =>
            {
                if (string.IsNullOrEmpty(novPisec.Ime))
                {
                    return Results.BadRequest("Ime avtorja je obvezno!");
                }

                var maxId = 0;
                foreach (var avtor in DataContext.VsiAvtorji)
                {
                    if (avtor.Id > maxId) maxId = avtor.Id;
                }
                novPisec.Id = maxId + 1;

                DataContext.VsiAvtorji.Add(novPisec);

                return Results.Ok(new
                {
                    sporocilo = "Avtor uspešno dodan!",
                    avtor = novPisec
                });
            })
            .WithName("DodajPisca");
        }
    }
}