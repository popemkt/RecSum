using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RecSum.Domain.Invoice;
using RecSum.Domain.Invoice.Dtos;
using RecSum.Logic;

namespace RecSum.Controllers;

[ApiController]
[Route("invoices")]
public class InvoiceController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ImportInvoices([FromBody] List<ImportInvoiceDto> dtos,
        [FromServices] IHandler<ImportInvoiceCommand, bool> importCommandHandler,
        [FromServices] IValidator<ImportInvoiceCommand> validator)
    {
        var importInvoiceCommand = new ImportInvoiceCommand().WithDtos(dtos);
        var validationResult = await validator.ValidateAsync(importInvoiceCommand);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return await importCommandHandler.Handle(importInvoiceCommand) ? Ok() : Conflict("No resource was created");
    }

    [HttpGet("summary")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SummaryDto>> GetInvoiceSummary([FromQuery] InvoiceSummaryQuery query,
        [FromServices] IHandler<InvoiceSummaryQuery, SummaryDto> summaryQueryHandler,
        [FromServices] IValidator<InvoiceSummaryQuery> validator)
    {
        var validationResult = await validator.ValidateAsync(query);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return Ok(await summaryQueryHandler.Handle(query));
    }
}