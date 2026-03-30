using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore; 
using Vaja4NalogaTest;

namespace Vaja4NalogaTest
{
    public static class EndPoint
    {
        public static void ConfigureEndpoints(WebApplication app)
        {
            app.MapGet("/api/Knjige", () =>
            {
                using(var db = new PdBaza())
                {
                    return db.VseKnjige.ToList();
                }
            }).WithTags("Knjige")
                .WithSummary("Izpise vse Knjige");

            app.MapGet("/api/Knjige/id/{id}", (int id) =>
            {
                
                using (var db = new PdBaza())
                {
                    var knj = db.VseKnjige.Find(id);
                    if ( knj == null)
                    {
                        return Results.NotFound("Ni Ida");
                    }
                    
                    return Results.Ok(knj);
                }
            }).WithTags("Knjige")
            .WithSummary("Iskanje Knjig z id-em");;
            
            app.MapGet("/api/Knjige/vse", () =>
            {
                using(var db = new PdBaza())
                {
                    // Uporaba .Include() izvede JOIN, da naloži tudi podatke Zaloznika
                    var knjige = db.VseKnjige.Include(k => k.Zaloznik).ToList();
                    return Results.Ok(knjige);
                }
            }).WithTags("Knjige").WithSummary("Izpiše vse knjige z vključenimi podatki o založniku.");
            
            app.MapPost("/api/Knjiga", (Knjiga novaKnjiga) =>
            {
                using (var db = new PdBaza())
                {
                    db.VseKnjige.Add(novaKnjiga);
                    db.SaveChanges();
                    return Results.Ok(novaKnjiga);
                }
            }).WithTags("Knjige")
                .WithSummary("Doda Knjigo po jason");

            app.MapDelete("/api/Knjige/id/{id}", (int id) =>
            {
                using (var db = new PdBaza())
                {
                    var knj = db.VseKnjige.Find(id);
                    if ( knj == null)
                    {
                        return Results.NotFound("Ni Ida");
                    }
                  
                    db.VseKnjige.Remove(knj);
                    db.SaveChanges();
                    return Results.Ok(knj);
                }
            }).WithTags("Knjige")
            .WithSummary("Izbrise Knjigo po jason");
            
            
            app.MapPut("/api/Knjige/id/{id}", (int id, Knjiga posodobljenaKnjiga) =>
            {
                using (var db = new PdBaza())
                {
                    // 1. Najdi obstoječo knjigo v bazi
                    var knjigaBaza = db.VseKnjige.Find(id);
        
                    if (knjigaBaza == null)
                    {
                        return Results.NotFound($"Knjiga z ID {id} ni najdena.");
                    }
        
                    // 2. Posodobi lastnosti z novimi podatki
                    knjigaBaza.Naslov = posodobljenaKnjiga.Naslov;
                    knjigaBaza.Avtor = posodobljenaKnjiga.Avtor;
                    knjigaBaza.DatumIzdelave = posodobljenaKnjiga.DatumIzdelave;
                    knjigaBaza.ZaloznikId = posodobljenaKnjiga.ZaloznikId; // Posodobitev tudi tujega ključa
        
                    // 3. Shrani spremembe
                    db.SaveChanges();
                    return Results.Ok(knjigaBaza);
                }
            }).WithTags("Knjige").WithSummary("Posodobi obstoječo knjigo po ID-ju.");
            
            
            
            
            // Zalozniki
            app.MapPost("/api/Zaloznik", (Zaloznik novZaloznik) =>
            {
                using (var db = new PdBaza())
                {
                    db.VsiZaložniki.Add(novZaloznik);
                    db.SaveChanges();
                    return Results.Created($"/api/Zaloznik/{novZaloznik}",novZaloznik);
                }
            }).WithTags("Zalozniki").WithSummary("Doda novega založnika.");

            app.MapGet("/api/Zalozniki", () =>
            {
                using (var db = new PdBaza())
                {
                    // Uporaba .Include() za nalaganje tudi vseh knjig, ki pripadajo založniku
                    var zalozniki = db.VsiZaložniki.Include(z => z.Knjige).ToList();
                    return Results.Ok(zalozniki);
                }

            }).WithTags("Zalozniki").WithSummary("Izpise vse Zaloznike z knjigami");

            app.MapGet("/api/Zaloznik", () =>
            {
                using (var db = new PdBaza())
                {
                    // Uporaba .Include() za nalaganje tudi vseh knjig, ki pripadajo založniku
                    var zalozniki = db.VsiZaložniki.ToList();
                    return Results.Ok(zalozniki);
                }

            }).WithTags("Zalozniki").WithSummary("Izpise vse Zaloznike ");

        }

    }
}