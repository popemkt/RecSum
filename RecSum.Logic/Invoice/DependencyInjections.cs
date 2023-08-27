using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecSum.Application.Invoice.Handlers;
using RecSum.Application.Invoice.Validations;
using RecSum.Domain.Configurations;
using RecSum.Domain.Invoice;

namespace RecSum.Application.Invoice;

public static class DependencyInjections
{
    public static IServiceCollection AddInvoiceServices(this IServiceCollection services, IConfigurationSection summaryConfig)
    {
        services.Configure<SummaryConfiguration>(summaryConfig);
        services.AddScoped<IHandler<InvoiceSummaryQuery, SummaryDto>, InvoiceSummaryQueryHandler>();
        services.AddScoped<IHandler<ImportInvoiceCommand, bool>, InvoiceImportCommandHandler>();
        services.AddTransient<ISummaryCalculator, SummaryCalculator>();
        services.AddTransient<IValidator<ImportInvoiceCommand>, ImportInvoiceValidator>();
        services.AddTransient<IValidator<InvoiceSummaryQuery>, InvoiceQueryValidator>();
        services.AddTransient<ISummaryCalculator, SummaryCalculator>();
        return services;
    }
}