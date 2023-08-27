using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RecSum.DataAccess;

public static class DependencyInjections
{
    public static IServiceCollection AddAppSqllite(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<RecSumContext>(options => options
            .UseSqlite($"Data Source={connectionString}"));
    }
}