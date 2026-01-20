using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LibraryService.DL.Interfaces;
using LibraryService.DL.Repositories;
using LibraryService.Models.Configurations;
using MongoDB.Driver;

namespace LibraryService.DL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services)
        {
            // MongoDB handles int32 natively, no custom serializer needed
            
            services.AddSingleton<IReaderRepository, ReaderRepository>();
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<ILoanRepository, LoanRepository>();
            return services;
        }

    

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDbConfiguration"));
            
            // Register MongoClient with connection string from configuration
            var mongoConfig = configuration.GetSection("MongoDbConfiguration").Get<MongoDbConfiguration>();
            if (mongoConfig != null)
            {
                services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConfig.ConnectionString));
            }
            else
            {
                services.AddSingleton<IMongoClient>(_ => new MongoClient());
            }
            
            return services;
        }

    }
}
