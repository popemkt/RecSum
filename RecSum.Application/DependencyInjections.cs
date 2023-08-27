using Microsoft.Extensions.DependencyInjection;
using RecSum.Application.Invoice;

namespace RecSum.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddInvoiceServices(this IServiceCollection services)
    {
        services.AddTransient<ISummaryCalculator, SummaryCalculator>();
        return services;
    }
}