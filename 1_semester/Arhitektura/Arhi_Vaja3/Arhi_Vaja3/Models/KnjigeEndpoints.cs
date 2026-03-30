using Arhi_Vaja3.Models;
using Arhi_Vaja3.Data;

namespace Arhi_Vaja3.Endpoints
{
    public static class KnjigeEndpoints
    {
        public static void MapKnjigeEndpoints(this WebApplication app)
        {
            // GET /api/knjige - Pridobi vse knjige
            app.MapGet("/api/knjige", () =>
            {
                return Results.Ok(DataContext.VseKnjige);
            })
            .WithName("GetVseKnjige")
            .WithOpenApi();

            // GET /api/knjige/{id} - Pridobi posamezno knjigo
            app.MapGet("/api/knjige/{id}", (int id) =>
            {
                var knjiga = DataContext.VseKnjige.FirstOrDefault(k => k.Id == id);
                if (knjiga == null)
                {
                    return Results.NotFound($"Knjiga z ID {id} ne obstaja.");
                }
                return Results.Ok(knjiga);
            })
            .WithName("GetKnjiga")
            .WithOpenApi();

            // GET /api/knjige/isci?naslov=prvi - Iskanje knjig
            app.MapGet("/api/knjige/isci", (string? naslov) =>
            {
                var vseKnjige = DataContext.VseKnjige;

                // Če je parameter prazen, vrni vse knjige
                if (string.IsNullOrEmpty(naslov))
                {
                    return Results.Ok(vseKnjige);
                }

                // Iskanje po naslovu
                var najdeneKnjige = new List<Knjige>();
                foreach (var knjiga in vseKnjige)
                {
                    if (knjiga.Naslob.ToLower().Contains(naslov.ToLower()))
                    {
                        najdeneKnjige.Add(knjiga);
                    }
                }

                return Results.Ok(najdeneKnjige);
            })
            .WithName("IsciKnjige");

            // POST /api/knjige - Dodaj novo knjigo
            app.MapPost("/api/knjige", (Knjige novaKnjiga) =>
            {
                // Preveri če je naslov prazen
                if (string.IsNullOrEmpty(novaKnjiga.Naslob))
                {
                    return Results.BadRequest("Naslov knjige je obvezen!");
                }

                // Poišči največji ID in dodaj 1
                var maxId = 0;
                foreach (var knjiga in DataContext.VseKnjige)
                {
                    if (knjiga.Id > maxId)
                    {
                        maxId = knjiga.Id;
                    }
                }
                novaKnjiga.Id = maxId + 1;

                // Dodaj knjigo v seznam
                DataContext.VseKnjige.Add(novaKnjiga);

                // Vrni potrditev
                return Results.Ok(new
                {
                    sporocilo = "Knjiga uspešno dodana!",
                    knjiga = novaKnjiga
                });
            })
            .WithName("DodajKnjigo");



            // PUT /api/knjige/{id} - Posodobi knjigo
            app.MapPut("/api/knjige/{id}", (int id, Knjige posodobljenaKnjiga) =>
            {
                // Poišči knjigo
                Knjige? knjigaZaPosodobitev = null;
                foreach (var knjiga in DataContext.VseKnjige)
                {
                    if (knjiga.Id == id)
                    {
                        knjigaZaPosodobitev = knjiga;
                        break;
                    }
                }

                // Če knjiga ne obstaja
                if (knjigaZaPosodobitev == null)
                {
                    return Results.NotFound($"Knjiga z ID {id} ne obstaja.");
                }

                // Posodobi podatke
                knjigaZaPosodobitev.Naslob = posodobljenaKnjiga.Naslob;
                knjigaZaPosodobitev.DatumObjave = posodobljenaKnjiga.DatumObjave;
                knjigaZaPosodobitev.Navoljo = posodobljenaKnjiga.Navoljo;

                return Results.Ok(new
                {
                    sporocilo = "Knjiga uspešno posodobljena!",
                    knjiga = knjigaZaPosodobitev
                });
            })
            .WithName("PosodobiKnjigo");

            // DELETE /api/knjige/{id} - Izbriši knjigo
            app.MapDelete("/api/knjige/{id}", (int id) =>
            {
                // Poišči knjigo
                Knjige? knjigaZaBrisanje = null;
                foreach (var knjiga in DataContext.VseKnjige)
                {
                    if (knjiga.Id == id)
                    {
                        knjigaZaBrisanje = knjiga;
                        break;
                    }
                }

                // Če knjiga ne obstaja
                if (knjigaZaBrisanje == null)
                {
                    return Results.NotFound($"Knjiga z ID {id} ne obstaja.");
                }

                // Izbriši knjigo
                DataContext.VseKnjige.Remove(knjigaZaBrisanje);

                return Results.Ok(new
                {
                    sporocilo = "Knjiga uspešno izbrisana!",
                    id = id
                });
            })
            .WithName("IzbrisiKnjigo");
        }
    }
}