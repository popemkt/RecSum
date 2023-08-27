using RecSum.Domain.Invoice.Dtos;

namespace RecSum.Domain.Invoice;

public class ImportInvoiceCommand
{
    public List<ImportInvoiceDto> Dtos { get; private set; }

    public ImportInvoiceCommand WithDtos(List<ImportInvoiceDto> dtos)
    {
        Dtos = dtos;
        return this;
    }
}