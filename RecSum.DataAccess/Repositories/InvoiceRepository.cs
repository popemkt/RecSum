using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecSum.Domain.Configurations;
using RecSum.Domain.Entities;
using RecSum.Domain.Invoice;
using RecSum.Domain.Repositories;

namespace RecSum.DataAccess.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly RecSumContext _context;
    private readonly SummaryConfiguration _summaryConfiguration;

    public InvoiceRepository(RecSumContext context,  IOptions<SummaryConfiguration> summaryConfiguration)
    {
        _context = context;
        _summaryConfiguration = summaryConfiguration.Value;
    }

    public Task<List<Invoice>> GetInvoiceSummaryAsync(InvoiceSummaryQuery query)
    {
        var today = DateTime.UtcNow.Date;
        
        return _context.Invoices.AsNoTracking()
            .Where(x => x.IssueDate >= today.AddMonths(-(query.MonthsToConsider ?? _summaryConfiguration.MonthsToConsider)))
            .ToListAsync();
    }
}