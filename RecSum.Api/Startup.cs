using System.Net;
using RecSum.Application;
using RecSum.DataAccess;
using RecSum.Domain.Configurations;
using RecSum.Infrastructure;

namespace RecSum;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddAppSqllite(Configuration.GetConnectionString("Sqlite"))
            .AddRepositories()
            .AddApplicationServices(Configuration.GetSection(SummaryConfiguration.SectionName))
            .AddInfrastructureServices();
    }

    public void Configure(IApplicationBuilder application)
    {
        application.UseExceptionHandler(app =>
        {
            app.Run(
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("Internal Server Error").ConfigureAwait(false);
                });
        });
        application.UseSwagger();
        application.UseSwaggerUI();
        application.UseRouting();
        application.UseEndpoints(e => e.MapControllers());
    }
}