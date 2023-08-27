using Microsoft.Extensions.DependencyInjection;
using RecSum.Application.Invoice;
using RecSum.DataAccess;
using RecSum.Infrastructure.Repositories;

namespace RecSum.Infrastructure;

public static class DependencyInjections
{
    public static IServiceCollection AddRecSumDataAccessServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrencyConverter, InMemoryCurrencyConverter>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        return services;
    }
}