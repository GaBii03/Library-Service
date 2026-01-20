using LibraryService.BL;
using LibraryService.DL;
using LibraryService.Host.Validators;
using LibraryService.Models.Configurations;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace LibraryService.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

            builder.Host.UseSerilog();
            builder.Services.AddValidatorsFromAssemblyContaining<AddReaderValidator>();

            // Add services to the container.
            builder.Services
                .AddConfigurations(builder.Configuration)
                .AddDataLayer()
                .AddBusinessLogicLayer();

            builder.Services.AddMapster();

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Service", Version = "v1" });
            });

            // Add Health Checks
            var mongoConfig = builder.Configuration.GetSection("MongoDbConfiguration").Get<MongoDbConfiguration>();
            if (mongoConfig != null)
            {
                builder.Services.AddHealthChecks()
                    .AddMongoDb(
                        sp => sp.GetRequiredService<IMongoClient>(),
                        name: "mongodb",
                        tags: new[] { "db", "mongodb" });
            }
            else
            {
                builder.Services.AddHealthChecks();
            }


            var app = builder.Build();
            // Configure the HTTP request pipeline.


            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            // Map Health Check endpoint
            app.MapHealthChecks("/health");

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "Library Service V1");
            });

            // Redirect root to Swagger
            app.MapGet("/", () => Results.Redirect("/swagger"));

            app.Run();
        }
    }
}
