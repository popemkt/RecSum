using Microsoft.Extensions.Logging;
using RecSum.Application.Invoice;
using RecSum.DataAccess;
using RecSum.Domain.Invoice;
using RecSum.Domain.Invoice.Dtos;

namespace RecSum.Logic.Invoice.Handlers;

public class InvoiceImportCommandHandler : IHandler<ImportInvoiceCommand, bool>
{
    private readonly ILogger<InvoiceImportCommandHandler> _logger;
    private readonly RecSumContext _context;

    public InvoiceImportCommandHandler(ILogger<InvoiceImportCommandHandler> logger, RecSumContext context)
    {
        _logger = logger;
        _context = context;
    }
    public async Task<bool> Handle(ImportInvoiceCommand message)
    {
        var dtos = message.Dtos;
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var references = dtos.Select(x => x.Reference);
            var savedEntities = _context.Invoices.Where(x => references.Contains(x.Reference)).ToList();
            // Skip saved entities for now, this shouldn't happen, or if it happens the dto needs LastUpdatedDate info to properly handle update 
            
            _context.AddRange(
                dtos
                    .ExceptBy(savedEntities.Select(x => x.Reference), x => x.Reference)
                    .Select(ImportInvoiceDto.ToEntity)
                    .ToList());

            var result = await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during import: {errorMessage}", ex.Message);
            await transaction.RollbackAsync();
            throw;
        }    }
}