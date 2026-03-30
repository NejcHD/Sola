using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace AIS_16_10_3.Routes
{
    public class Kosarice
    {
        public static void MapRoutes(WebApplication app)
        {
           
            app.MapGet("/api/kosarice", () =>
            {
                var db = new BazaContext();

                var vseKosarice = db.kosarice.ToList();


                return vseKosarice;
            }).WithTags("Kosarice")
              .WithSummary("Izpise vse Kosarice");


            app.MapPost("/api/kosarica", (BazaContext db) =>
            {
                var nova = new Kosarica();
                db.kosarice.Add(nova);
                db.SaveChanges();

                return Results.Ok("Dodano");

            }).WithTags("Kosarice")
              .WithSummary("Dodaj novo Kosarico");


            app.MapPost("/api/kosarica/{id}", (int id, BazaContext db, int artikelId) =>
            {
                var najdenaKosarica = db.kosarice.FirstOrDefault(k => k.Id == id);
                var najdenArtikel = db.artikli.FirstOrDefault(a => a.Id == artikelId);

                if (najdenaKosarica == null || najdenArtikel == null)
                {
                    return Results.NotFound("Id ne obstaja");
                }

                najdenaKosarica.SeznamArtiklov.Add(najdenArtikel);
                db.SaveChanges();

                return Results.Ok($"Artikel {najdenArtikel.Naziv} je bil dodan v košarico {id}.");
            }).WithTags("Kosarice")
            .WithSummary("Doda obstoječ artikel v določeno košarico");


            app.MapDelete("api/kosarica/{id}", (int id, BazaContext db) =>
            {
                var najdenakosarica = db.artikli.FirstOrDefault(k => k.Id == id);
                if (najdenakosarica == null)
                {
                    return Results.NotFound($"Ni najden kosarica z idem {id}");
                }

                db.artikli.Remove(najdenakosarica);
                db.SaveChanges();
                return Results.Ok("kosarica izbrisan");
            }).WithTags("Artikel").WithSummary("Izbrise kosarica z podanim id-em");




        }
    }
}
