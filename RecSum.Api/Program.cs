using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using RecSum.Application;
using RecSum.DataAccess;
using RecSum.Domain.Configurations;
using RecSum.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbPath = Path.Join(Directory.GetCurrentDirectory(), "rec-sum.db");

builder.Services.AddAppSqllite(dbPath)
    .AddRepositories()
    .AddApplicationServices(builder.Configuration.GetSection(SummaryConfiguration.SectionName))
    .AddInfrastructureServices();

var app = builder.Build();

app.UseExceptionHandler(app =>
{
    app.Run(
        async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("Internal Server Error").ConfigureAwait(false);
        });
});
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();