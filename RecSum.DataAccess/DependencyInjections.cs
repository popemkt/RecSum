using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecSum.DataAccess.Repositories;
using RecSum.Domain.Repositories;

namespace RecSum.DataAccess;

public static class DependencyInjections
{
    public static IServiceCollection AddAppSqllite(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<RecSumContext>(options => options
            .UseSqlite($"Data Source={connectionString}"));
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        return services;
    }
}