using RecSum.Domain.Invoice;

namespace RecSum.Application.Invoice;

public interface ISummaryCalculator
{
    SummaryDto GetSummary(List<NormalizedInvoiceDto> invoices);
}