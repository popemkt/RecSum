using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RecSum.DataAccess;

namespace RecSum.IntegrationTests.Infrastructure;

public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<RecSumContext>));
            services.Remove(descriptor);
            
            string dbName = $"InMemoryDbForTesting_{new Random().NextInt64(long.MaxValue)}";
            services.AddDbContext<RecSumContext>(options =>
            {
                options.UseInMemoryDatabase(dbName)
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        });
    }
}