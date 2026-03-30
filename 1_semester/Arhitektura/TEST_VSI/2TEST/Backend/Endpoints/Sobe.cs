using Backend.Entitete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Backend.Endpoints
{
    public class Sobe
    {
        public static void MapApi(WebApplication app)
        {
            app.MapGet("/sobe",
            [EndpointSummary("Pridobi seznam sob")]
            [EndpointDescription("Vrne vse sobe skupaj z rezervacijami.")]
            [ProducesResponseType(typeof(List<Soba>), StatusCodes.Status200OK)] async () =>
            {
                await using var db = new BazaContext();
                var sobe = await db.Sobe.Include(s=>s.Rezervacije).ToListAsync();
                return Results.Ok(sobe);
            }).WithTags("Sobe");

            app.MapGet("/sobe/{id:int}",
            [EndpointSummary("Pridobi sobo po ID")]
            [EndpointDescription("Vrne sobo z določenim ID, vključno z rezervacijami.")]
            [ProducesResponseType(typeof(Soba), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)] async ([Description("ID sobe")] int id) =>
            {
                await using var db = new BazaContext();
                var soba = await db.Sobe.Include(s=>s.Rezervacije).FirstOrDefaultAsync(s => s.Id == id);
                return soba is null ? Results.NotFound() : Results.Ok(soba);
            }).WithTags("Sobe");

            app.MapPost("/sobe",
            [EndpointSummary("Ustvari novo sobo")]
            [EndpointDescription("Ustvari novo sobo na podlagi posredovanih podatkov.")]
            [ProducesResponseType(typeof(Soba), StatusCodes.Status201Created)] async ([Description("Podatki nove sobe")] SobaDto dto) =>
            {
                await using var db = new BazaContext();
                var soba = new Soba
                {
                    Stevilka = dto.Stevilka,
                    Kapaciteta = dto.Kapaciteta,
                    CenaNaNoc = dto.CenaNaNoc
                };

                db.Sobe.Add(soba);
                await db.SaveChangesAsync();
                return Results.Created($"/sobe/{soba.Id}", soba);
            }).WithTags("Sobe");

            app.MapPut("/sobe/{id:int}",
            [EndpointSummary("Posodobi sobo")]
            [EndpointDescription("Posodobi obstoječo sobo z določenim ID.")]
            [ProducesResponseType(typeof(Soba), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)] async ([Description("ID sobe")] int id, [Description("Posodobljeni podatki sobe")] SobaDto input) =>
            {
                await using var db = new BazaContext();
                var soba = await db.Sobe.FindAsync(id);
                if (soba is null)
                {
                    return Results.NotFound();
                }

                soba.Stevilka = input.Stevilka;
                soba.Kapaciteta = input.Kapaciteta;
                soba.CenaNaNoc = input.CenaNaNoc;

                await db.SaveChangesAsync();
                return Results.Ok(soba);
            }).WithTags("Sobe");

            app.MapDelete("/sobe/{id:int}",
            [EndpointSummary("Izbriši sobo")]
            [EndpointDescription("Izbriše sobo z določenim ID.")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)] async ([Description("ID sobe")] int id) =>
            {
                await using var db = new BazaContext();
                var soba = await db.Sobe.FindAsync(id);
                if (soba is null)
                {
                    return Results.NotFound();
                }

                db.Sobe.Remove(soba);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }).WithTags("Sobe");
        }
    }
}
