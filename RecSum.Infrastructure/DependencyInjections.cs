using Microsoft.Extensions.DependencyInjection;
using RecSum.Domain.Invoice;

namespace RecSum.Infrastructure;

public static class DependencyInjections
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrencyConverter, InMemoryCurrencyConverter>();
        return services;
    }
}