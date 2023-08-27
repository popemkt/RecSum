using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecSum.Application.Invoice;

namespace RecSum.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfigurationSection summarySection)
    {
        services.AddInvoiceServices(summarySection);
        return services;
    }
}