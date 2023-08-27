using RecSum.Domain.Invoice;

namespace RecSum.Domain.Repositories;

public interface IInvoiceRepository
{
    Task<List<Domain.Entities.Invoice>> GetInvoiceSummaryAsync(InvoiceSummaryQuery query);
}