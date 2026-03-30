using Microsoft.AspNetCore.Builder;
using System.Runtime.CompilerServices;

namespace AAA
{
    public static class StundetuEndPoint
    {

        public static void IzpisStudenta(this WebApplication app)
        {

            app.MapGet("/api/Student", () =>
            {
                return Results.Ok(Podatki.VsiStudentje);
            });


            app.MapGet("/api/Student/Id/{Id}", (int Id) =>
            {
                var student = Podatki.VsiStudentje.FirstOrDefault(k => k.ID == Id);
                if (student == null)
                {
                    return Results.NotFound($"Student z ID {Id} ne obstaja.");
                }

                return Results.Ok(student);

            });

            app.MapGet("/api/Student/Name/{Name}", (string Name) =>
            {
                var Ime = Podatki.VsiStudentje.FirstOrDefault(k => k.Name == Name);

                if (Ime == null)
                {
                    return Results.NotFound($"Student z Imenom:  {Name} ne obstaja.");
                }

                return Results.Ok(Ime);
            });


            app.MapPost("/api/Student/", (Student noviStudent) =>
            {
                if (string.IsNullOrEmpty(noviStudent.Name))
                {
                    return Results.BadRequest("Naslov knjige je obvezen!");  //preveri ce je vpisan naslov v /postu
                }

                var maxId = 0;
                foreach(var Student in Podatki.VsiStudentje)
                {
                    if(Student.ID > maxId)
                    {
                        maxId = Student.ID;
                    }
                }

                noviStudent.ID = maxId + 1;

                Podatki.VsiStudentje.Add(noviStudent);
                return Results.Ok(new
                {
                    sporocilo = "Student uspešno dodana!",
                    Student = noviStudent
                });

            });

            app.MapPut("/api/Student/ID/{ID}", (int id, Student PosodobljenStudent) =>
            {
                var student = Podatki.VsiStudentje.FirstOrDefault(k => k.ID == id);
                if(student == null)
                {
                    return Results.NotFound("Nig u are retardet");
                }

                student.Name = PosodobljenStudent.Name;
                student.age = PosodobljenStudent.age;


                return Results.Ok(student);

            });


            app.MapDelete("/api/Student/ID/{ID}", (int id) =>
            {
                var student = Podatki.VsiStudentje.FirstOrDefault(k => k.ID == id);
                if(student == null)
                {
                    return Results.NotFound("Stupid ass niga");
                };
                Student StudentZaIzbris = null;
                foreach (var Student in Podatki.VsiStudentje)
                {
                    if(Student.ID == id)
                    {
                         StudentZaIzbris = Student;
                    }
                }

                Podatki.VsiStudentje.Remove(StudentZaIzbris);

                return Results.Ok(new
                {
                    sporocilo = "Student je bil terminiran",
                    ID = id
                });
            });


        }
    }
}
