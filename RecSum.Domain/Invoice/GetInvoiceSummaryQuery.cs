namespace RecSum.Domain.Invoice;

public class GetInvoiceSummaryQuery
{
    public int? MonthsToConsider {
        get;
        set;
    }
    
    public string? Currency { get; set; }
}