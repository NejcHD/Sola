using BBB;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<PodatkiPB>(); /// tooo

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PodatkiPB>();
    db.NapolniBazo(); // <-- TO JE VSE!
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.IzpisStudenta();  /// to

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
