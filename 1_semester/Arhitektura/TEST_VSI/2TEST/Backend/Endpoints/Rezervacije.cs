using Backend.Entitete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Backend.Endpoints
{
    public class Rezervacije
    {
        public static void MapApi(WebApplication app)
        {
            app.MapGet("/rezervacije",
            [EndpointSummary("Pridobi seznam rezervacij")]
            [EndpointDescription("Vrne vse rezervacije skupaj s pripadajočimi sobami.")]
            [ProducesResponseType(typeof(List<Rezervacija>), StatusCodes.Status200OK)] async () =>
            {
                await using var db = new BazaContext();
                var rezervacije = await db.Rezervacije.Include(r => r.Soba).ToListAsync();
                return Results.Ok(rezervacije);
            }).WithTags("Rezervacije");

            app.MapGet("/rezervacije/{id:int}",
            [EndpointSummary("Pridobi rezervacijo po ID")]
            [EndpointDescription("Vrne rezervacijo z določenim ID, vključno s sobo.")]
            [ProducesResponseType(typeof(Rezervacija), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)] async ([Description("ID rezervacije")] int id) =>
            {
                await using var db = new BazaContext();
                var rezervacija = await db.Rezervacije.Include(r => r.Soba).FirstOrDefaultAsync(r => r.Id == id);
                return rezervacija is null ? Results.NotFound() : Results.Ok(rezervacija);
            }).WithTags("Rezervacije");

            app.MapPost("/rezervacije",
            [EndpointSummary("Ustvari novo rezervacijo")]
            [EndpointDescription("Ustvari novo rezervacijo za izbrano sobo.")]
            [ProducesResponseType(typeof(Rezervacija), StatusCodes.Status201Created)]
            [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)] async ([Description("Podatki nove rezervacije")] RezervacijaDto dto) =>
            {
                await using var db = new BazaContext();

                if (string.IsNullOrWhiteSpace(dto.ImeGosta))
                {
                    return Results.BadRequest("Ime gosta je obvezno.");
                }

                if (dto.Od > dto.Do)
                {
                    return Results.BadRequest("Datum 'Od' mora biti pred ali enak datumu 'Do'.");
                }

                // Check if the referenced room exists
                var soba = await db.Sobe.FindAsync(dto.SobaId);
                if (soba is null)
                {
                    return Results.BadRequest($"Soba z Id {dto.SobaId} ne obstaja.");
                }

                var prekrivanje = await db.Rezervacije.AnyAsync(r =>
                    r.SobaId == dto.SobaId &&
                    dto.Od <= r.Do &&
                    dto.Do >= r.Od);
                if (prekrivanje)
                {
                    return Results.BadRequest("Rezervacija se prekriva z obstoječo rezervacijo za isto sobo.");
                }

                var rezervacija = new Rezervacija
                {
                    Od = dto.Od,
                    Do = dto.Do,
                    ImeGosta = dto.ImeGosta,
                    Opomba = dto.Opomba,
                    SobaId = dto.SobaId
                };
                
                db.Rezervacije.Add(rezervacija);
                await db.SaveChangesAsync();
                return Results.Created($"/rezervacije/{rezervacija.Id}", rezervacija);
            }).WithTags("Rezervacije");


            app.MapPut("/rezervacije/{id:int}",
            [EndpointSummary("Posodobi rezervacijo")]
            [EndpointDescription("Posodobi obstoječo rezervacijo z določenim ID.")]
            [ProducesResponseType(typeof(Rezervacija), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)] async ([Description("ID rezervacije")] int id, [Description("Posodobljeni podatki rezervacije")] RezervacijaDto input) =>
            {
                await using var db = new BazaContext();
                var rezervacija = await db.Rezervacije.FindAsync(id);
                if (rezervacija is null)
                {
                    return Results.NotFound();
                }

                if (string.IsNullOrWhiteSpace(input.ImeGosta))
                {
                    return Results.BadRequest("Ime gosta je obvezno.");
                }

                if (input.Od > input.Do)
                {
                    return Results.BadRequest("Datum 'Od' mora biti pred ali enak datumu 'Do'.");
                }

                var soba = await db.Sobe.FindAsync(input.SobaId);
                if (soba is null)
                {
                    return Results.BadRequest($"Soba z Id {input.SobaId} ne obstaja.");
                }

                var prekrivanje = await db.Rezervacije.AnyAsync(r =>
                    r.SobaId == input.SobaId &&
                    r.Id != id &&
                    input.Od <= r.Do &&
                    input.Do >= r.Od);
                if (prekrivanje)
                {
                    return Results.BadRequest("Rezervacija se prekriva z obstoječo rezervacijo za isto sobo.");
                }

                rezervacija.Od = input.Od;
                rezervacija.Do = input.Do;
                rezervacija.ImeGosta = input.ImeGosta;
                rezervacija.Opomba = input.Opomba;
                rezervacija.SobaId = input.SobaId;

                await db.SaveChangesAsync();
                return Results.Ok(rezervacija);
            }).WithTags("Rezervacije");

            app.MapDelete("/rezervacije/{id:int}",
            [EndpointSummary("Izbriši rezervacijo")]
            [EndpointDescription("Izbriše rezervacijo z določenim ID.")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)] async ([Description("ID rezervacije")] int id) =>
            {
                await using var db = new BazaContext();
                var rezervacija = await db.Rezervacije.FindAsync(id);
                if (rezervacija is null)
                {
                    return Results.NotFound();
                }

                db.Rezervacije.Remove(rezervacija);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }).WithTags("Rezervacije");
        }
    }
}
