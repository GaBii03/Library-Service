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
            
            services.AddSingleton<IReaderRepository, ReaderRepository>();
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<ILoanRepository, LoanRepository>();
            return services;
        }

    

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDbConfiguration"));
            
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
