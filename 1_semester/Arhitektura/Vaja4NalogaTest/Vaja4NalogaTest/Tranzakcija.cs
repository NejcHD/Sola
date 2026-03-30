using Microsoft.EntityFrameworkCore;
using Vaja4NalogaTest;



namespace Vaja4NalogaTest
{
    public static class Tranzakcija
    {
        public class ZaloznikInKnjiga
        {
            public Zaloznik NovZaloznik { get; set; }
            public Knjiga NovaKnjiga { get; set; }
        }
        
        public static void ConfigureEndpoints(WebApplication app)
        {
            app.MapPost("/api/Tranzakcija/UstvariZaloznikaInKnjigo", (ZaloznikInKnjiga model) =>
            {
                using (var db = new PdBaza())
                {
                    using var transaction = db.Database.BeginTransaction();

                    try
                    {
                        db.VsiZaložniki.Add(model.NovZaloznik);
                        db.SaveChanges();
                        
                        // KLJUČNA POVEZAVA: Povežemo knjigo z ID-jem, ki ga je ravnokar dobil založnik
                        model.NovaKnjiga.ZaloznikId = model.NovZaloznik.Id;
                        
                        db.VseKnjige.Add(model.NovaKnjiga);
                        db.SaveChanges();
                        
                        transaction.Commit(); 
                            
                        return Results.Created("/api/Knjige", model);  
                            
                    }catch
                    {
                        transaction.Rollback(); 
                        return Results.Problem($"Napaka pri transakciji. V bazi ni ostalo nič.");
                    }
                }
            }) .WithTags("Transakcije").WithSummary("Atomsko ustvari novega založnika in novo knjigo v eni transakciji.");
        }
        
    }
}

