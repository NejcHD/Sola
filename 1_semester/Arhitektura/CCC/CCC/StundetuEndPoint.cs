using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace CCC
{
    public static class StundetuEndPoint
    {

        public static void IzpisStudenta(this WebApplication app)
        {

            app.MapGet("/api/Student", async (PodatkiPB db) =>
            {
                var vsiStudenti = await db.VsiStudentje.ToListAsync();
                return Results.Ok(vsiStudenti);
            })
               .WithTags("02 - Študenti (CRUD)") // Dodajanje značke
               .WithSummary("Pridobi seznam vseh študentov.") // Kratek opis
               .Produces(200, typeof(List<Student>)) ;



            app.MapGet("/api/Student/ID/{id}",  (int id) =>
            {
                using (var db = new PodatkiPB())
                {
                    var student = db.VsiStudentje.Find(id);
                    if (student == null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(student);
                }

            })
                .WithTags("02 - Študenti (CRUD)")
                .WithSummary("Pridobi študenta glede na ID.")
                .Produces(200, typeof(Student)) // Uspešen odgovor
                .Produces(404) ; // Ne najden odgovor;

            app.MapGet("/api/Student/Name/{Name}", (string Name) =>
            {
                using (var db = new PodatkiPB())
                {
                    var student = db.VsiStudentje.FirstOrDefault(k => k.Name == Name);
                    if (student == null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(student);
                }

            })  .WithTags("02 - Študenti (CRUD)")
                .WithSummary("Pridobi študenta glede na ime.")
                .Produces(200, typeof(Student))
                .Produces(404, typeof(void)); ;

            app.MapPost("/api/Student", async (Student novStudent) =>
            {

                using (var db = new PodatkiPB())
                {
                    db.VsiStudentje.Add(novStudent);
                    db.SaveChangesAsync();
                }

                return Results.Ok(new
                {
                    message = "Student dodan",
                    id = novStudent.ID,
                    student = novStudent
                });

            })  .WithTags("02 - Študenti (CRUD)")
                .WithSummary("Doda novega študenta v bazo.")
                .Produces(200, typeof(object))
                .Produces(400, typeof(void)); ;

            app.MapPut("/api/Student/ID/{id}", async(int id, Student PosodobljenStudnet) =>
            {
                using (var db = new PodatkiPB())
                {
                    var ObstojecStudent = db.VsiStudentje.Find(id);
                    if(ObstojecStudent == null)
                    {
                        return Results.NotFound("ur a faggot");
                    }

                    ObstojecStudent.Name = PosodobljenStudnet.Name;
                    ObstojecStudent.age = PosodobljenStudnet.age;
                    db.SaveChangesAsync();

                    return Results.Ok("good niga");
                }

            })  .WithTags("02 - Študenti (CRUD)")
                .WithSummary("Posodobi obstoječega študenta glede na ID.")
                .Produces(200, typeof(string))
                .Produces(404, typeof(string));
            ;


            app.MapDelete("/api/Student/ID/{id}", async (int id) =>
            {
                using (var db = new PodatkiPB())
                {
                    var NajdiStudenta = db.VsiStudentje.Find(id);
                    if(NajdiStudenta == null)
                    {
                        return Results.NotFound("stupid ah niga");
                    }
                    db.VsiStudentje.Remove(NajdiStudenta);
                    db.SaveChangesAsync();

                    return Results.Ok("Gud boy");
                }

            });

        }
    }
}
