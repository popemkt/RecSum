namespace RecSum.Domain.Invoice.Dtos;

public class ImportInvoiceDto
{
    public string Reference { get; set; }
    public string CurrencyCode { get; set; }
    public DateTime IssueDate { get; set; }
    public double OpeningValue { get; set; }
    public double PaidValue { get; set; }
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

    public static Entities.Invoice ToEntity(ImportInvoiceDto dto)
    {
        return new Entities.Invoice
        {
            Reference = dto.Reference,
            CurrencyCode = dto.CurrencyCode,
            IssueDate = dto.IssueDate,
            OpeningValue = dto.OpeningValue,
            PaidValue = dto.PaidValue,
            DueDate = dto.DueDate,
            ClosedDate = dto.ClosedDate,
            Cancelled = dto.Cancelled,
            DebtorName = dto.DebtorName,
            DebtorReference = dto.DebtorReference,
            DebtorAddress1 = dto.DebtorAddress1,
            DebtorAddress2 = dto.DebtorAddress2,
            DebtorTown = dto.DebtorTown,
            DebtorState = dto.DebtorState,
            DebtorZip = dto.DebtorZip,
            DebtorCountryCode = dto.DebtorCountryCode,
            DebtorRegistrationNumber = dto.DebtorRegistrationNumber
        };
    }
}