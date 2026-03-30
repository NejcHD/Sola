using Backend.Entitete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Backend.Endpoints
{
    public class Sporocila
    {
        public static void MapApi(WebApplication app)
        {

            /*
            Pridobi seznam sporočil
            200 OK: List<Sporocilo>
            */
            app.MapGet("/sporocila", async (BazaContext db) =>
            {
               
                var sporocila = await db.Sporocila
                    .ToListAsync();
                return Results.Ok(sporocila);
            }).WithTags("Sporocilo").WithSummary("Vrne vsa sporocila");

            /*
            Pridobi sporočilo po ID
            Vrne eno sporočilo skupaj s pošiljateljem in prejemnikom.
            OK: Sporocilo
            Not Found
            */

            app.MapGet("/sporocila/{id:int}",
                async (int id) =>
            {
                
                
                var iskanNabiralnik = new Nabiralnik();
                if(iskanNabiralnik.Aktiven != true)
                {
                    return Results.NotFound("Nabiralnik ni najden");   
                }
                
                await using var db = new BazaContext();
                var sporocilo = await db.Sporocila
                    .FirstOrDefaultAsync(s => s.Id == id);

                return sporocilo is null ? Results.NotFound() : Results.Ok(sporocilo);
            }).WithTags("Sporocilo").WithSummary("Izpis sporocila gelde na id");


            app.MapPost("/api/sporocila", async([FromBody]  Sporocilo novoSporocilo,   BazaContext db) =>
            {

                

                if(novoSporocilo == null)
                {
                    return Results.NotFound("Ni Dodanega sporocila");
                }


                db.Sporocila.Add(novoSporocilo);
                db.SaveChanges();

                
                return Results.Ok("Dodano");
            }).WithTags("Sporocilo").WithSummary("Dodajanje sporocila");



         



        }
    }
}
