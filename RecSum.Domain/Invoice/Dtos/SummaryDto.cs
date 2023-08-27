namespace RecSum.Domain.Invoice;

public class SummaryDto
{
    public int DataRecencyInMonths { get; set; }
    public string CurrencyCode { get; set; }
    public double TotalInvoiceValue { get; set; }
    public int NumberOfDebtors { get; set; }
    public int NumberOfOpenInvoices { get; set; }
    public int NumberOfClosedInvoices { get; set; }
    public double? AverageInvoiceValue { get; set; }
    public double TotalPaidValue { get; set; }
    public int NumberOfCancelledInvoices { get; set; }
    public int NumberOfOpenOverdueInvoices { get; set; }
    public double? AveragePaymentPeriod { get; set; }
    public double TotalOpenOverdueAmount { get; set; }
    public double? AverageOpenOverduePeriod { get; set; }
    public Dictionary<string, double> InvoiceValueDistribution { get; set; }
    public Dictionary<string, double> OutstandingValueDistribution { get; set; }
    public double ClosedInvoicesValue { get; set; }
    public double OpenInvoicesOutstandingValue { get; set; }
}