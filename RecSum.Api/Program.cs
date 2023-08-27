using RecSum.Application;
using RecSum.DataAccess;
using RecSum.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbPath = System.IO.Path.Join(Directory.GetCurrentDirectory(), "rec-sum.db");
builder.Services.AddAppSqllite(dbPath);
builder.Services.AddRecSumDataAccessServices();
builder.Services.AddInvoiceServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();