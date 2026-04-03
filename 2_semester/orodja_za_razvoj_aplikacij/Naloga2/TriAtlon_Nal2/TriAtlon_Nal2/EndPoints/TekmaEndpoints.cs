using Microsoft.EntityFrameworkCore;
using Projekt;
using Projekt.Models;

namespace TriAtlon_Nal2.EndPoints
{
    public static class TekmaEndpoints
    {
        public static void MapTekmaEndpoints(this IEndpointRouteBuilder app)
        {
            // PRIDOBI VSE TEKME
            app.MapGet("/api/tekma", async (ApplicationDbContext db) =>
            {
                return Results.Ok(await db.Tekma.ToListAsync());
            })
            .WithTags("Tekma")
            .WithSummary("Izpise seznam tekem iz baze.");

            // DODAJ NOVO TEKMO
            app.MapPost("/api/tekma", async (Tekma novaTekma, ApplicationDbContext db) =>
            {
                db.Tekma.Add(novaTekma);
                await db.SaveChangesAsync();
                return Results.Created($"/api/tekma/{novaTekma.idTekma}", novaTekma);
            })
            .WithTags("Tekma")
            .WithSummary("Doda  tekemo v baze.");
        }
    }
}