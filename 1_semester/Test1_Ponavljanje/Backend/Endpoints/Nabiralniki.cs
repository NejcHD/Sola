using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Backend.Entitete;

namespace Backend.Endpoints
{
    public class Nabiralniki
    {
        public static void MapApi(WebApplication app)
        {
            /*
            Pridobi seznam nabiralnikov
            Vrne vse nabiralnike skupaj s prejetimi in poslanimi sporočili.
            */
            app.MapGet("/nabiralniki", async () =>
            {
                await using var db = new BazaContext();
                var nabiralniki = await db.Nabiralniki
                    .ToListAsync();
                Console.WriteLine("Vračam nabiralnike!");
                return Results.Ok(nabiralniki);
            });

            /*
            Pridobi nabiralnik po emailu
            Vrne en nabiralnik skupaj s prejetimi in poslanimi sporočili.
            */

            app.MapGet("/nabiralniki/{email}", async (string email) =>
            {
                await using var db = new BazaContext();
                var nabiralnik = await db.Nabiralniki
                .Include(x => x.PoslanaSporocila)
                .Include(x => x.PrejetaSporocila)
                    .FirstOrDefaultAsync(n => n.Email == email);

                return nabiralnik is null ? Results.NotFound() : Results.Ok(nabiralnik);
            });
        }
    }
}
