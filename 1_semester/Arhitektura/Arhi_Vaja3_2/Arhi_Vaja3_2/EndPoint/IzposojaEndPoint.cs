using Arhi_Vaja3.Models;
using Arhi_Vaja3.Data;

namespace Arhi_Vaja3.Endpoints
{
    public static class IzposojeEndpoints
    {
        public static void MapIzposojeEndpoints(this WebApplication app)
        {
            // GET /api/izposoje - Pridobi vse izposoje
            app.MapGet("/api/izposoje", () =>
            {
                var rezultat = new List<object>();

                foreach (var izposoja in DataContext.VseIzposoje)
                {
                    var knjiga = DataContext.VseKnjige.FirstOrDefault(k => k.Id == izposoja.IdKnjige);
                    var avtor = DataContext.VsiAvtorji.FirstOrDefault(a => a.Id == izposoja.IdAvtorja);

                    rezultat.Add(new
                    {
                        izposoja.Id,
                        izposoja.DatumIzposoje,
                        izposoja.DatumVrnitve,
                        izposoja.ImeIzposodbe,
                        Knjiga = knjiga,
                        Avtor = avtor,
                        JeAktivna = izposoja.DatumVrnitve == null
                    });
                }

                return Results.Ok(rezultat);
            })
            .WithName("GetVseIzposoje")
            .WithOpenApi();

            // GET /api/izposoje/{id} - Pridobi posamezno izposojo
            app.MapGet("/api/izposoje/{id}", (int id) =>
            {
                var izposoja = DataContext.VseIzposoje.FirstOrDefault(i => i.Id == id);
                if (izposoja == null)
                {
                    return Results.NotFound($"Izposoja z ID {id} ne obstaja.");
                }

                var knjiga = DataContext.VseKnjige.FirstOrDefault(k => k.Id == izposoja.IdKnjige);
                var avtor = DataContext.VsiAvtorji.FirstOrDefault(a => a.Id == izposoja.IdAvtorja);

                var rezultat = new
                {
                    izposoja.Id,
                    izposoja.DatumIzposoje,
                    izposoja.DatumVrnitve,
                    izposoja.ImeIzposodbe,
                    Knjiga = knjiga,
                    Avtor = avtor,
                    JeAktivna = izposoja.DatumVrnitve == null
                };

                return Results.Ok(rezultat);
            })
            .WithName("GetIzposoja")
            .WithOpenApi();

            // GET /api/izposoje/isci - Iskanje izposoj
            app.MapGet("/api/izposoje/isci", (string? ime, bool? aktivne) =>
            {
                var vseIzposoje = DataContext.VseIzposoje;

                // Če so vsi parametri prazni, vrni vse izposoje
                if (string.IsNullOrEmpty(ime) && aktivne == null)
                {
                    return Results.Ok(PripraviRezultatIzposoj(vseIzposoje));
                }

                // Iskanje po parametrih
                var najdeneIzposoje = new List<Izposoja>();
                foreach (var izposoja in vseIzposoje)
                {
                    var ustreza = true;

                    // Preveri ime
                    if (!string.IsNullOrEmpty(ime) && !izposoja.ImeIzposodbe.ToLower().Contains(ime.ToLower()))
                    {
                        ustreza = false;
                    }

                    // Preveri aktivnost
                    if (aktivne.HasValue)
                    {
                        var jeAktivna = izposoja.DatumVrnitve == null;
                        if (aktivne.Value != jeAktivna)
                        {
                            ustreza = false;
                        }
                    }

                    if (ustreza)
                    {
                        najdeneIzposoje.Add(izposoja);
                    }
                }

                return Results.Ok(PripraviRezultatIzposoj(najdeneIzposoje));
            })
            .WithName("IsciIzposoje");

            // POST /api/izposoje - Dodaj novo izposojo
            app.MapPost("/api/izposoje", (Izposoja novaIzposoja) =>
            {
                // Preveri če so obvezni podatki
                if (string.IsNullOrEmpty(novaIzposoja.ImeIzposodbe))
                {
                    return Results.BadRequest("Ime izposojevalca je obvezno!");
                }

                // Preveri če knjiga obstaja
                var knjiga = DataContext.VseKnjige.FirstOrDefault(k => k.Id == novaIzposoja.IdKnjige);
                if (knjiga == null)
                {
                    return Results.NotFound($"Knjiga z ID {novaIzposoja.IdKnjige} ne obstaja.");
                }

                // Preveri če avtor obstaja
                var avtor = DataContext.VsiAvtorji.FirstOrDefault(a => a.Id == novaIzposoja.IdAvtorja);
                if (avtor == null)
                {
                    return Results.NotFound($"Avtor z ID {novaIzposoja.IdAvtorja} ne obstaja.");
                }

                // Preveri če je knjiga že izposojena
                foreach (var izposoja in DataContext.VseIzposoje)
                {
                    if (izposoja.IdKnjige == novaIzposoja.IdKnjige && izposoja.DatumVrnitve == null)
                    {
                        return Results.BadRequest("Knjiga je že izposojena!");
                    }
                }

                // Poišči največji ID in dodaj 1
                var maxId = 0;
                foreach (var izposoja in DataContext.VseIzposoje)
                {
                    if (izposoja.Id > maxId)
                    {
                        maxId = izposoja.Id;
                    }
                }
                novaIzposoja.Id = maxId + 1;

                // Nastavi datum izposoje na danes
                novaIzposoja.DatumIzposoje = DateTime.Now;
                novaIzposoja.DatumVrnitve = null;

                // Dodaj izposojo v seznam
                DataContext.VseIzposoje.Add(novaIzposoja);

                // Označi knjigo kot izposojeno
                knjiga.Navoljo = false;

                // Vrni potrditev
                return Results.Ok(new
                {
                    sporocilo = "Izposoja uspešno dodana!",
                    izposoja = novaIzposoja,
                    knjiga = knjiga,
                    avtor = avtor
                });
            })
            .WithName("DodajIzposojo");

            // PUT /api/izposoje/{id}/vrni - Vrni knjigo (special endpoint)
            app.MapPut("/api/izposoje/{id}/vrni", (int id) =>
            {
                // Poišči izposojo
                Izposoja? izposojaZaVrnitev = null;
                foreach (var izposoja in DataContext.VseIzposoje)
                {
                    if (izposoja.Id == id)
                    {
                        izposojaZaVrnitev = izposoja;
                        break;
                    }
                }

                // Če izposoja ne obstaja
                if (izposojaZaVrnitev == null)
                {
                    return Results.NotFound($"Izposoja z ID {id} ne obstaja.");
                }

                // Preveri če je knjiga že vrnjena
                if (izposojaZaVrnitev.DatumVrnitve != null)
                {
                    return Results.BadRequest("Knjiga je že vrnjena!");
                }

                // Nastavi datum vrnitve na danes
                izposojaZaVrnitev.DatumVrnitve = DateTime.Now;

                // Označi knjigo kot na voljo
                var knjiga = DataContext.VseKnjige.FirstOrDefault(k => k.Id == izposojaZaVrnitev.IdKnjige);
                if (knjiga != null)
                {
                    knjiga.Navoljo = true;
                }

                return Results.Ok(new
                {
                    sporocilo = "Knjiga uspešno vrnjena!",
                    izposoja = izposojaZaVrnitev
                });
            })
            .WithName("VrniKnjigo");

            // PUT /api/izposoje/{id} - Posodobi izposojo
            app.MapPut("/api/izposoje/{id}", (int id, Izposoja posodobljenaIzposoja) =>
            {
                // Poišči izposojo
                Izposoja? izposojaZaPosodobitev = null;
                foreach (var izposoja in DataContext.VseIzposoje)
                {
                    if (izposoja.Id == id)
                    {
                        izposojaZaPosodobitev = izposoja;
                        break;
                    }
                }

                // Če izposoja ne obstaja
                if (izposojaZaPosodobitev == null)
                {
                    return Results.NotFound($"Izposoja z ID {id} ne obstaja.");
                }

                // Preveri obvezne podatke
                if (string.IsNullOrEmpty(posodobljenaIzposoja.ImeIzposodbe))
                {
                    return Results.BadRequest("Ime izposojevalca je obvezno!");
                }

                // Posodobi podatke
                izposojaZaPosodobitev.ImeIzposodbe = posodobljenaIzposoja.ImeIzposodbe;
                izposojaZaPosodobitev.DatumIzposoje = posodobljenaIzposoja.DatumIzposoje;
                izposojaZaPosodobitev.DatumVrnitve = posodobljenaIzposoja.DatumVrnitve;

                return Results.Ok(new
                {
                    sporocilo = "Izposoja uspešno posodobljena!",
                    izposoja = izposojaZaPosodobitev
                });
            })
            .WithName("PosodobiIzposojo");

            // DELETE /api/izposoje/{id} - Izbriši izposojo
            app.MapDelete("/api/izposoje/{id}", (int id) =>
            {
                // Poišči izposojo
                Izposoja? izposojaZaBrisanje = null;
                foreach (var izposoja in DataContext.VseIzposoje)
                {
                    if (izposoja.Id == id)
                    {
                        izposojaZaBrisanje = izposoja;
                        break;
                    }
                }

                // Če izposoja ne obstaja
                if (izposojaZaBrisanje == null)
                {
                    return Results.NotFound($"Izposoja z ID {id} ne obstaja.");
                }

                // Izbriši izposojo
                DataContext.VseIzposoje.Remove(izposojaZaBrisanje);

                return Results.Ok(new
                {
                    sporocilo = "Izposoja uspešno izbrisana!",
                    id = id
                });
            })
            .WithName("IzbrisiIzposojo");
        }

        // Pomožna metoda za pripravo rezultata
        private static List<object> PripraviRezultatIzposoj(List<Izposoja> izposoje)
        {
            var rezultat = new List<object>();

            foreach (var izposoja in izposoje)
            {
                var knjiga = DataContext.VseKnjige.FirstOrDefault(k => k.Id == izposoja.IdKnjige);
                var avtor = DataContext.VsiAvtorji.FirstOrDefault(a => a.Id == izposoja.IdAvtorja);

                rezultat.Add(new
                {
                    izposoja.Id,
                    izposoja.DatumIzposoje,
                    izposoja.DatumVrnitve,
                    izposoja.ImeIzposodbe,
                    Knjiga = knjiga,
                    Avtor = avtor,
                    JeAktivna = izposoja.DatumVrnitve == null
                });
            }

            return rezultat;
        }
    }
}