using GrpcOseba;

var builder = WebApplication.CreateBuilder(args);

// Dodajanje storitev za gRPC
builder.Services.AddGrpc();

var app = builder.Build();

// Konfiguracija HTTP zahtev (Za development mora biti HTTPS)
app.UseHttpsRedirection();

// Registracija naše CRUD storitve
app.MapGrpcService<OsebeServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();