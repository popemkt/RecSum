namespace RecSum.Domain.Invoice;

public class InvoiceSummaryQuery
{
    public int? MonthsToConsider {
        get;
        set;
    }
    
    public string? Currency { get; set; }
}