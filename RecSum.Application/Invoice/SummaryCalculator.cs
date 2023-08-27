using RecSum.Domain.Invoice;

namespace RecSum.Application.Invoice;

public class SummaryCalculator : ISummaryCalculator
{
    public SummaryDto GetSummary(List<NormalizedInvoiceDto> invoices)
    {
        var today = DateTime.UtcNow.Date;

        var invoicesNotCancelled = invoices.Where(x => x.Cancelled != true).ToList();
        var closedInvoices = invoicesNotCancelled.Where(x => x.ClosedDate.HasValue).ToList();
        var openInvoices = invoicesNotCancelled.Where(x => !x.ClosedDate.HasValue).ToList();
        var openOverdueInvoices = openInvoices.Where(x => x.DueDate < today).ToList();

        return new SummaryDto()
        {
            NumberOfDebtors = invoices.Select(x => x.DebtorReference).Distinct().Count(),
            TotalInvoiceValue = invoicesNotCancelled.Sum(x => x.NormalizedOpeningValue),
            AverageInvoiceValue = invoicesNotCancelled.Any()
                ? invoicesNotCancelled.Select(x => x.NormalizedOpeningValue).Average()
                : null,
            AveragePaymentPeriod = closedInvoices.Any()
                ? closedInvoices.Select(x => (x.ClosedDate!.Value - x.IssueDate).Days).Average()
                : null,
            NumberOfClosedInvoices = closedInvoices.Count,
            ClosedInvoicesValue = closedInvoices.Sum(x => x.NormalizedOpeningValue),
            NumberOfOpenInvoices = openInvoices.Count,
            OpenInvoicesOutstandingValue = openInvoices.Sum(x => x.NormalizedOpeningValue - x.NormalizedPaidValue),
            NumberOfOpenOverdueInvoices = openOverdueInvoices.Count,
            TotalOpenOverdueAmount = openOverdueInvoices.Sum(x => x.NormalizedOpeningValue - x.NormalizedPaidValue),
            AverageOpenOverduePeriod = openOverdueInvoices.Any()
                ? openOverdueInvoices.Select(x => (x.DueDate - today).Days).Average()
                : null,
            TotalPaidValue = invoicesNotCancelled.Sum(x => x.NormalizedPaidValue),
            NumberOfCancelledInvoices = invoices.Count(x => x.Cancelled == true),
            InvoiceValueDistribution = invoicesNotCancelled.GroupBy(x => x.DebtorCountryCode)
                .ToDictionary(x => x.Key,
                    x => x.Sum(x => x.NormalizedOpeningValue)),
            OutstandingValueDistribution = invoicesNotCancelled.Where(x => x.ClosedDate is null)
                .GroupBy(x => x.DebtorCountryCode)
                .ToDictionary(x => x.Key,
                    x => x.Sum(x => x.NormalizedOpeningValue - x.NormalizedPaidValue)),
        };
    }
}