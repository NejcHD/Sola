using Microsoft.AspNetCore.Mvc;

namespace AIS_16_10_3.Routes
{
    public static class Artikli
    {
        public static void MapRoutes(this WebApplication app)
        {
            // 1. GET VSI
            app.MapGet("/artikli", (BazaContext db) =>
            {
                var artikli = db.artikli.ToList();
                return Results.Ok(artikli);
            }).WithTags("Artikel").WithSummary("Izpise vse artikle");



            // 2. GET MIN/MAX
            app.MapGet("/artikli/MinMax", (BazaContext db, decimal? min, decimal? max) => {
                var query = db.artikli.AsQueryable();
                if (min.HasValue) query = query.Where(a => a.Cena >= min.Value);
                if (max.HasValue) query = query.Where(a => a.Cena <= max.Value);
                return Results.Ok(query.ToList());
            });



            // 3. POST (TUKAJ JE BILA NAPAKA - dodan BazaContext db)
            app.MapPost("/artikli", ([FromBody] Artikel artikel, BazaContext db) =>
            {
                db.artikli.Add(artikel);
                db.SaveChanges();
                return Results.Created($"/artikli/{artikel.Id}", artikel);
            }).WithTags("Artikel").WithSummary("Doda artikel");




            // 4. DELETE (Poenostavljeno z BazaContext db)
            app.MapDelete("api/artikli/{id}", (int id, BazaContext db) =>
            {
                var najdenArtikel = db.artikli.FirstOrDefault(k => k.Id == id);
                if (najdenArtikel == null)
                {
                    return Results.NotFound($"Ni najden artikel z idem {id}");
                }

                db.artikli.Remove(najdenArtikel);
                db.SaveChanges();
                return Results.Ok("Artikel izbrisan");
            }).WithTags("Artikel").WithSummary("Izbrise artikel z podanim id-em");




            // 5. PUT (Tvoj že popravljen del)
            app.MapPut("api/artikli/ID/{id}", (int id, [FromBody] Artikel posodobljen, BazaContext db) =>
            {
                var ObstojecArtikel = db.artikli.FirstOrDefault(k => k.Id == id);
                if (ObstojecArtikel == null)
                {
                    return Results.NotFound($"Ni najden artikel z idem {id}");
                }

                ObstojecArtikel.Naziv = posodobljen.Naziv;
                ObstojecArtikel.Cena = posodobljen.Cena;
                ObstojecArtikel.Opis = posodobljen.Opis;
                db.SaveChanges();

                return Results.Ok("Posodobljen");
            }).WithTags("Artikel").WithSummary("Posodobi artikel z id-em");
        }
    }
}