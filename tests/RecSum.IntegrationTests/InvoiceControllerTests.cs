using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop.Infrastructure;
using RecSum.DataAccess;
using RecSum.Domain.Configurations;
using RecSum.Domain.Constants;
using RecSum.Domain.Invoice;
using RecSum.Domain.Invoice.Dtos;
using RecSum.IntegrationTests.Infrastructure;
using Shouldly;

namespace RecSum.IntegrationTests;

public class InvoiceControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
{
    private readonly TestWebApplicationFactory<Startup> _factory;
    private readonly HttpClient _client;
    private readonly Faker _faker;
    private readonly IServiceProvider _serviceProvider;

    public InvoiceControllerTests(TestWebApplicationFactory<Startup> factory)
    {
        _faker = new Faker();
        _factory = factory;
        _serviceProvider = _factory.Services;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task ImportInvoices_WithValidData_ShouldSucceedAndSaveCorrectData()
    {
        var dtos = Enumerable.Range(1, 10).Select(_ => new ImportInvoiceDto()
        {
            Reference = Guid.NewGuid().ToString(),
            DebtorReference = Guid.NewGuid().ToString(),
            CurrencyCode = CurrencyCode.VND,
            DebtorName = _faker.Name.FullName(),
            DebtorCountryCode = _faker.Address.CountryCode(),
            IssueDate = DateTime.UtcNow.Date,
            DueDate = DateTime.UtcNow.Date.AddDays(1),
            Cancelled = true,
            OpeningValue = 10000,
            DebtorAddress1 = _faker.Address.FullAddress(),
            DebtorAddress2 = _faker.Address.FullAddress(),
            DebtorTown = _faker.Address.City(),
            DebtorZip = _faker.Address.ZipCode(),
            DebtorState = _faker.Address.State(),
            DebtorRegistrationNumber = _faker.Finance.Bic(),
        }).ToList();


        var result = await _client.PostAsJsonAsync("/invoices", dtos);

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RecSumContext>();

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var dbInvoices = await context.Invoices.ToListAsync();
        dbInvoices.Count.ShouldBe(10);
        foreach (var dto in dtos)
        {
            var dbInvoice = dbInvoices.Single(x => x.Reference == dto.Reference);
            dbInvoice.Reference.ShouldBe(dto.Reference);
            dbInvoice.DebtorReference.ShouldBe(dto.DebtorReference);
            dbInvoice.CurrencyCode.ShouldBe(dto.CurrencyCode);
            dbInvoice.DebtorName.ShouldBe(dto.DebtorName);
            dbInvoice.DebtorCountryCode.ShouldBe(dto.DebtorCountryCode);
            dbInvoice.IssueDate.ShouldBe(dto.IssueDate);
            dbInvoice.DueDate.ShouldBe(dto.DueDate);
            dbInvoice.Cancelled.ShouldBe(dto.Cancelled);
            dbInvoice.OpeningValue.ShouldBe(dto.OpeningValue);
            dbInvoice.DebtorAddress1.ShouldBe(dto.DebtorAddress1);
            dbInvoice.DebtorAddress2.ShouldBe(dto.DebtorAddress2);
            dbInvoice.DebtorTown.ShouldBe(dto.DebtorTown);
            dbInvoice.DebtorZip.ShouldBe(dto.DebtorZip);
            dbInvoice.DebtorState.ShouldBe(dto.DebtorState);
        }
    }


    [Fact]
    public async Task SummaryQuery_ShouldSucceed()
    {
        using var scope = _serviceProvider.CreateScope();
        var dtos = Enumerable.Range(1, 10).Select(_ => new ImportInvoiceDto()
        {
            Reference = Guid.NewGuid().ToString(),
            DebtorReference = Guid.NewGuid().ToString(),
            CurrencyCode = CurrencyCode.VND,
            DebtorName = _faker.Name.FullName(),
            DebtorCountryCode = _faker.Address.CountryCode(),
            IssueDate = DateTime.UtcNow.Date,
            DueDate = DateTime.UtcNow.Date.AddDays(1),
            Cancelled = false,
            OpeningValue = 10000,
            DebtorAddress1 = _faker.Address.FullAddress(),
            DebtorAddress2 = _faker.Address.FullAddress(),
            DebtorTown = _faker.Address.City(),
            DebtorZip = _faker.Address.ZipCode(),
            DebtorState = _faker.Address.State(),
            DebtorRegistrationNumber = _faker.Finance.Bic(),
        }).ToList();

        var result = await _client.PostAsJsonAsync("/invoices", dtos);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var summaryResult = await _client.GetAsync("/invoices/summary");
        summaryResult.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SummaryQuery_ShouldCorrectlySummarize()
    {
        using var scope = _serviceProvider.CreateScope();
        var dtos = new List<ImportInvoiceDto>()
        {
            //Open
            new()
            {
                Reference = Guid.NewGuid().ToString(),
                DebtorReference = Guid.NewGuid().ToString(),
                CurrencyCode = CurrencyCode.VND,
                DebtorName = _faker.Name.FullName(),
                DebtorCountryCode = "VN",
                IssueDate = DateTime.UtcNow.Date,
                DueDate = DateTime.UtcNow.Date.AddDays(1),
                Cancelled = false,
                OpeningValue = 20000,
                DebtorAddress1 = _faker.Address.FullAddress(),
                DebtorAddress2 = _faker.Address.FullAddress(),
                DebtorTown = _faker.Address.City(),
                DebtorZip = _faker.Address.ZipCode(),
                DebtorState = _faker.Address.State(),
                DebtorRegistrationNumber = _faker.Finance.Bic(),
            },
            new()
            {
                //Overdue
                Reference = Guid.NewGuid().ToString(),
                DebtorReference = Guid.NewGuid().ToString(),
                CurrencyCode = CurrencyCode.USD,
                DebtorName = _faker.Name.FullName(),
                DebtorCountryCode = "UK",
                IssueDate = DateTime.UtcNow.Date,
                DueDate = DateTime.UtcNow.Date.AddDays(-10),
                Cancelled = false,
                OpeningValue = 10000,//After converting will be 20000
                DebtorAddress1 = _faker.Address.FullAddress(),
                DebtorAddress2 = _faker.Address.FullAddress(),
                DebtorTown = _faker.Address.City(),
                DebtorZip = _faker.Address.ZipCode(),
                DebtorState = _faker.Address.State(),
                DebtorRegistrationNumber = _faker.Finance.Bic(),
            },
            //Closed
            new()
            {
                Reference = Guid.NewGuid().ToString(),
                DebtorReference = Guid.NewGuid().ToString(),
                CurrencyCode = CurrencyCode.VND,
                DebtorName = _faker.Name.FullName(),
                DebtorCountryCode = "VN2",
                IssueDate = DateTime.UtcNow.Date.AddMonths(-3),
                DueDate = DateTime.UtcNow.Date.AddDays(1),
                ClosedDate = DateTime.UtcNow.Date.AddDays(-1),
                Cancelled = false,
                OpeningValue = 10000,
                DebtorAddress1 = _faker.Address.FullAddress(),
                DebtorAddress2 = _faker.Address.FullAddress(),
                DebtorTown = _faker.Address.City(),
                DebtorZip = _faker.Address.ZipCode(),
                DebtorState = _faker.Address.State(),
                DebtorRegistrationNumber = _faker.Finance.Bic(),
            },
            new()
            {
                Reference = Guid.NewGuid().ToString(),
                DebtorReference = Guid.NewGuid().ToString(),
                CurrencyCode = CurrencyCode.USD,
                DebtorName = _faker.Name.FullName(),
                DebtorCountryCode = "UK2",
                IssueDate = DateTime.UtcNow.Date.AddMonths(-5),
                DueDate = DateTime.UtcNow.Date.AddDays(1),
                ClosedDate = DateTime.UtcNow.Date.AddMonths(-1),
                Cancelled = false,
                OpeningValue = 5000,//After converting will be 10000
                DebtorAddress1 = _faker.Address.FullAddress(),
                DebtorAddress2 = _faker.Address.FullAddress(),
                DebtorTown = _faker.Address.City(),
                DebtorZip = _faker.Address.ZipCode(),
                DebtorState = _faker.Address.State(),
                DebtorRegistrationNumber = _faker.Finance.Bic(),
            },
        };

        var result = await _client.PostAsJsonAsync("/invoices", dtos);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var config = scope.ServiceProvider.GetRequiredService<IOptions<SummaryConfiguration>>();
        
        var summaryResult = await _client.GetAsync("/invoices/summary");
        summaryResult.StatusCode.ShouldBe(HttpStatusCode.OK);
        var summary = await summaryResult.GetContentAsync<SummaryDto>();
        summary.AverageInvoiceValue.ShouldBe(15000);
        summary.AverageOpenOverduePeriod.ShouldBe(10);
        summary.AveragePaymentPeriod.ShouldBe(106.5);
        summary.ClosedInvoicesValue.ShouldBe(20000);
        summary.CurrencyCode.ShouldBe(config.Value.DefaultCurrencyCode);
        summary.DataRecencyInMonths.ShouldBe(config.Value.MonthsToConsider);
        summary.InvoiceValueDistribution["VN"].ShouldBeEquivalentTo(Math.Round(1f/3, 2));
        summary.InvoiceValueDistribution["UK"].ShouldBeEquivalentTo(Math.Round(1f/3, 2));
        summary.InvoiceValueDistribution["VN2"].ShouldBeEquivalentTo(Math.Round(1f/6, 2));
        summary.InvoiceValueDistribution["VN2"].ShouldBeEquivalentTo(Math.Round(1f/6, 2));
        summary.OutstandingValueDistribution["VN"].ShouldBeEquivalentTo(Math.Round(1f/2, 2));
        summary.OutstandingValueDistribution["UK"].ShouldBeEquivalentTo(Math.Round(1f/2, 2));
        summary.NumberOfCancelledInvoices.ShouldBe(0);
        summary.NumberOfClosedInvoices.ShouldBe(2);
        summary.NumberOfDebtors.ShouldBe(4);
        summary.NumberOfOpenInvoices.ShouldBe(2);
        summary.NumberOfOpenOverdueInvoices.ShouldBe(1);
        summary.OpenInvoicesOutstandingValue.ShouldBe(40000);
        summary.TotalInvoiceValue.ShouldBe(60000);
    }
}