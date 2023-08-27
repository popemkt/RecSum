using Microsoft.AspNetCore.Mvc;
using RecSum.Application.Invoice;
using RecSum.Domain.Invoice;
using RecSum.Domain.Invoice.Dtos;

namespace RecSum.Controllers;

[ApiController]
[Route("invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceController(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    [HttpPost]
    public Task ImportInvoices(List<ImportInvoiceDto> dtos)
    {
        return _invoiceRepository.ImportInvoicesAsync(dtos);
    }
    
    [HttpGet]
    public async Task<SummaryDto> GetInvoiceSummary([FromQuery]GetInvoiceSummaryQuery query)
    {
        return await _invoiceRepository.GetInvoiceSummaryAsync(query);
    }
}