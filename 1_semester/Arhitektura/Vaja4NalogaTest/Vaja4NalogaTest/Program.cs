using Vaja4NalogaTest;

var builder = WebApplication.CreateBuilder(args);

// **SAMO TO** za Swashbuckle:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var db = new PdBaza())
{
    db.UstvariBazo();
    
    if (!db.VsiZaložniki.Any())
    {
        db.VsiZaložniki.AddRange(
            new Zaloznik { Name = "Založba Modrijan" },
            new Zaloznik { Name = "Mladinska Knjiga" }
        );
        db.SaveChanges();
    }
    
    if (!db.VseKnjige.Any())
    {
        db.VseKnjige.AddRange(
            new Knjiga
            {
                Naslov = "Potovanje na luno",
                Avtor = "Jules Verne",
                DatumIzdelave = new DateTime(1865, 1, 1),
                ZaloznikId = 2
            }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    // **SAMO TO** za Swashbuckle:
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

EndPoint.ConfigureEndpoints(app);
Tranzakcija.ConfigureEndpoints(app);

app.Run();