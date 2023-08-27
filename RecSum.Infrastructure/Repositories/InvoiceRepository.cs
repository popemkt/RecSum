using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecSum.Application.Invoice;
using RecSum.DataAccess;
using RecSum.Domain.Configurations;
using RecSum.Domain.Entities;
using RecSum.Domain.Invoice;
using RecSum.Domain.Invoice.Dtos;

namespace RecSum.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ILogger<InvoiceRepository> _logger;
    private readonly RecSumContext _context;
    private readonly ICurrencyConverter _currencyConverter;
    private readonly ISummaryCalculator _summaryCalculator;
    private readonly SummaryConfiguration _summaryConfiguration;

    public InvoiceRepository(ILogger<InvoiceRepository> logger,RecSumContext context, ICurrencyConverter currencyConverter, ISummaryCalculator summaryCalculator, IOptions<SummaryConfiguration> summaryConfiguration)
    {
        _logger = logger;
        _context = context;
        _currencyConverter = currencyConverter;
        _summaryCalculator = summaryCalculator;
        _summaryConfiguration = summaryConfiguration.Value;
    }

    public async Task ImportInvoicesAsync(List<ImportInvoiceDto> dtos)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var references = dtos.Select(x => x.Reference);
            var savedEntities = _context.Invoices.Where(x => references.Contains(x.Reference)).ToList();
            // Skip saved entities for now, this shouldn't happen, or if it happens the dto needs LastUpdatedDate info to properly handle update 
            
            _context.AddRange(
                dtos
                .ExceptBy(savedEntities.Select(x => x.Reference), x => x.Reference)
                .Select(ImportInvoiceDto.ToEntity)
                .ToList());
            
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during import:", ex.Message);
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<SummaryDto> GetInvoiceSummaryAsync(GetInvoiceSummaryQuery query)
    {
        var today = DateTime.UtcNow.Date;
        
        var invoices = await _context.Invoices
            .Where(x => x.IssueDate >= today)
            .ToListAsync();

        var destinationCurrency = query.Currency ?? _summaryConfiguration.DefaultCurrencyCode;
        var monthsToConsider = query.MonthsToConsider ?? _summaryConfiguration.MonthsToConsider;
        
        var normalizedInvoices = invoices.Select(x =>
        {
            var dto = NormalizedInvoiceDto.FromEntity(x);
            if (x.CurrencyCode != destinationCurrency)
            {
                x.CurrencyCode = destinationCurrency;
                var ratio = _currencyConverter.GetRatio(x.CurrencyCode, destinationCurrency);
                dto.NormalizedOpeningValue = x.OpeningValue * ratio;
                dto.NormalizedPaidValue *= x.PaidValue * ratio;
            }
            else
            {
                dto.NormalizedOpeningValue = x.OpeningValue;
                dto.NormalizedPaidValue = x.PaidValue;
            }

            return dto;
        });

        var summary = _summaryCalculator.GetSummary(normalizedInvoices.ToList());
        summary.CurrencyCode = destinationCurrency;
        summary.DataRecencyInMonths = monthsToConsider;
        return summary;
    }
}