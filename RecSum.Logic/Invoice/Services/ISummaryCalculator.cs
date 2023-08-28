using RecSum.Domain.Invoice;

namespace RecSum.Logic.Invoice.Services;

public interface ISummaryCalculator
{
    SummaryDto GetSummary(List<NormalizedInvoiceDto> invoices);
}