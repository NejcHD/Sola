using Microsoft.EntityFrameworkCore;
using Projekt;


namespace TriAtlon_Nal2.EndPoints
{
    public static class DrzavaEndpoints
    {
        public static void MapDrzavaEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapGet("/api/drzava", async (ApplicationDbContext db) =>
            {
                return Results.Ok(await db.Drzava.ToListAsync());
            })
            .WithTags("Drzava")
            .WithSummary("Izpise seznam drzav iz baze.");


            app.MapGet("/api/drzava/{id}", async (int id, ApplicationDbContext db) =>
            {
                var najdenaDrzava = await db.Drzava.FindAsync(id);

                if (najdenaDrzava == null)
                {
                    return Results.NotFound("Iskana drzava z id-em ni bila najdena");
                }

                return Results.Ok(najdenaDrzava);
            })
            .WithTags("Drzava")
            .WithSummary("Izpise drzavo po id-u.");
        }
    }
}
