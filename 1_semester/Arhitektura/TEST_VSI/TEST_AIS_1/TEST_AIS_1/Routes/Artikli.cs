using Microsoft.AspNetCore.Mvc;

namespace AIS_16_10_3.Routes
{
    public static class Artikli
    {

        public static void MapRoutes(this WebApplication app)
        {
            app.MapGet("/artikli", () =>
            {
                var db = new BazaContext();

                var artikli = db.artikli.ToList();


                return Results.Ok(artikli);
            }).WithTags("Artikel")
              .WithSummary("Izpise vse artikle");

            app.MapPost("/artikli", (Artikel artikel) =>
            {
                var db = new BazaContext();
                db.artikli.Add(artikel);
                db.SaveChanges();
                return Results.Created($"/artikli/{artikel.Id}", artikel);
            }) .WithTags("Artikel")
               .WithSummary("Doda artikel"); 

            app.MapDelete("api/artikli/", (int id) =>
            {
                using (var db = new BazaContext())
                {
                    var najdenArtikel = db.artikli.FirstOrDefault(k => k.Id == id);
                    if(najdenArtikel == null)
                    {
                        Results.NotFound($"Ni najden artikel z idem {id}");                     
                    }

                    db.artikli.Remove(najdenArtikel);
                    db.SaveChanges();
                    return Results.Ok("Artikel izbrisan");

                }
            }).WithTags("Artikel")
              .WithSummary("Izbrise artikel z podanim id-em");

            app.MapPut("api/artikli/ID/{id}", (int id, int NovaCena, string noviNaziv, string Opis) =>
            {
                using (var db = new BazaContext())
                {
                    var ObstojecArtikel = db.artikli.FirstOrDefault(k => k.Id == id);
                    if (ObstojecArtikel == null)
                    {
                        Results.NotFound($"Ni najden artikel z idem {id}");
                    }

                    ObstojecArtikel.Naziv = noviNaziv;
                    ObstojecArtikel.Cena = NovaCena;
                    //ObstojecArtikel.Opis = Opis;
                    db.SaveChanges();

                    return Results.Ok("Posodobljen");

                }

            }).WithTags("Artikel")
              .WithSummary("Posodobi artikel z id-em");


        }

    }
}
