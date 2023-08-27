using RecSum.Domain.Invoice;
using RecSum.Domain.Invoice.Dtos;

namespace RecSum.Application.Invoice;

public interface IInvoiceRepository
{
    Task ImportInvoicesAsync(List<ImportInvoiceDto> dtos);
    Task<SummaryDto> GetInvoiceSummaryAsync(GetInvoiceSummaryQuery query);
}