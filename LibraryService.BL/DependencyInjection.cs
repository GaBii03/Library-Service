using LibraryService.BL.Interfaces;
using LibraryService.BL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryService.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            // Register BL services here
            services.AddSingleton<IReaderService, ReaderService>();
            services.AddSingleton<IBookService, Services.BookService>();
            services.AddSingleton<ILoanService, LoanService>();
            return services;
        }
    }
}
