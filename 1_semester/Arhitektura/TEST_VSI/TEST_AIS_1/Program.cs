
using AIS_16_10_3.Routes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AIS_16_10_3
{
    public class Program
    {
        // Dobrodošli na 1. testu!
        // Želim vam vso srečo pri reševanju nalog. :)
        // Lep pozdrav,
        // Alen Rajšp

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<BazaContext>();

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo  
                {
                    Title = "Artikli API",
                    Version = "v1"
                });
            });  

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            Artikli.MapRoutes(app);
            Kosarice.MapRoutes(app);




            app.Run();
        }
    }
}
