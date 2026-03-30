using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace BBB
{
    public static class StundetuEndPoint
    {

        public static void IzpisStudenta(this WebApplication app)
        {

            app.MapGet("/api/Student", async (PodatkiPB db) =>
            {
                var vsiStudenti = await db.VsiStudentje.ToListAsync();
                return Results.Ok(vsiStudenti);
            });

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

            });

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

            });

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

            });

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

            });


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
