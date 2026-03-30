using CCC;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();     /// to za swageer
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CCC API",
        Version = "v1",
        Description = "API za študente in transakcije"
    });
});

builder.Services.AddDbContext<PodatkiPB>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PodatkiPB>();
    db.NapolniBazo(); // <-- TO JE VSE!
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.TranzakcijaIzpis();  ///too

app.IzpisStudenta();   ///tooo

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
