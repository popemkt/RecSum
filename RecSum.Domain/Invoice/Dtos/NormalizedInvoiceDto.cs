namespace RecSum.Domain.Invoice;

public class NormalizedInvoiceDto
{
    public string Reference { get; set; }
    public string OriginalCurrencyCode { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool? Cancelled { get; set; }
    public string DebtorName { get; set; }
    public string DebtorReference { get; set; }
    public string? DebtorAddress1 { get; set; }
    public string? DebtorAddress2 { get; set; }
    public string? DebtorTown { get; set; }
    public string? DebtorState { get; set; }
    public string? DebtorZip { get; set; }
    public string DebtorCountryCode { get; set; }
    public string? DebtorRegistrationNumber { get; set; }
    public double NormalizedOpeningValue { get; set; }
    public double NormalizedPaidValue { get; set; }

    public static NormalizedInvoiceDto FromEntity(Entities.Invoice entity)
    {
        return new NormalizedInvoiceDto 
        {
            Reference = entity.Reference,
            OriginalCurrencyCode = entity.CurrencyCode,
            IssueDate = entity.IssueDate,
            DueDate = entity.DueDate,
            ClosedDate = entity.ClosedDate,
            Cancelled = entity.Cancelled,
            DebtorName = entity.DebtorName,
            DebtorReference = entity.DebtorReference,
            DebtorAddress1 = entity.DebtorAddress1,
            DebtorAddress2 = entity.DebtorAddress2,
            DebtorTown = entity.DebtorTown,
            DebtorState = entity.DebtorState,
            DebtorZip = entity.DebtorZip,
            DebtorCountryCode = entity.DebtorCountryCode,
            DebtorRegistrationNumber = entity.DebtorRegistrationNumber,
        };
    }
}