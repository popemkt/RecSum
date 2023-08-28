using Microsoft.Extensions.Options;
using RecSum.Application.Invoice;
using RecSum.Domain.Configurations;
using RecSum.Domain.Invoice;
using RecSum.Domain.Repositories;
using RecSum.Logic.Invoice.Services;

namespace RecSum.Logic.Invoice.Handlers;

public class InvoiceSummaryQueryHandler : IHandler<InvoiceSummaryQuery, SummaryDto>
{
    private readonly ISummaryCalculator _summaryCalculator;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICurrencyConverter _currencyConverter;
    private readonly SummaryConfiguration _summaryConfiguration;

    public InvoiceSummaryQueryHandler(ISummaryCalculator summaryCalculator, IInvoiceRepository invoiceRepository, IOptions<SummaryConfiguration> summaryConfiguration, ICurrencyConverter currencyConverter)
    {
        _summaryCalculator = summaryCalculator;
        _invoiceRepository = invoiceRepository;
        _currencyConverter = currencyConverter;
        _summaryConfiguration = summaryConfiguration.Value;
    }
    public async Task<SummaryDto> Handle(InvoiceSummaryQuery query)
    {
        var invoices = await _invoiceRepository.GetInvoiceSummaryAsync(query);
        
        var destinationCurrency = query.Currency ?? _summaryConfiguration.DefaultCurrencyCode;
        var monthsToConsider = query.MonthsToConsider ?? _summaryConfiguration.MonthsToConsider;
        
        var normalizedInvoices = invoices.Select(x =>
        {
            var dto = NormalizedInvoiceDto.FromEntity(x);
            if (!x.CurrencyCode.Equals(destinationCurrency, StringComparison.InvariantCulture))
            {
                var ratio = _currencyConverter.GetRatio(x.CurrencyCode, destinationCurrency);
                dto.NormalizedOpeningValue = x.OpeningValue * ratio;
                dto.NormalizedPaidValue *= x.PaidValue * ratio;
                x.CurrencyCode = destinationCurrency;
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