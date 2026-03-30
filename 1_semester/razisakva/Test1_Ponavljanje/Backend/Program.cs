
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<BazaContext>();

            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                     policy =>
                     {
                         policy.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                     });
            });
            builder.WebHost.UseUrls("http://localhost:5000");
            builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
                options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
                );

            var app = builder.Build();



            app.MapOpenApi();
            app.MapScalarApiReference();
            app.UseSwagger();
            app.UseSwaggerUI();

            MockData.ResetAndSeed();

            Endpoints.Nabiralniki.MapApi(app);
            Endpoints.Sporocila.MapApi(app);

            app.MapGet("/", () => "API Deluje!");


            app.Run();
        }
    }
}
