using FluentValidation;
using RecSum.Domain.Constants;
using RecSum.Domain.Errors;
using RecSum.Domain.Invoice;
using RecSum.Domain.Invoice.Dtos;

namespace RecSum.Application.Invoice.Validations;

public class ImportInvoiceValidator : AbstractValidator<ImportInvoiceCommand>
{
    public ImportInvoiceValidator()
    {
        RuleFor(x => x.Dtos).NotEmpty();
        RuleForEach(x => x.Dtos)
            .SetValidator(new ImportInvoiceDtoValidator())
            .When(x => x.Dtos.Any());
    }
}


public class ImportInvoiceDtoValidator : AbstractValidator<ImportInvoiceDto>
{
    public ImportInvoiceDtoValidator()
    {
        RuleFor(x => x.Reference).NotEmpty();
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().Must(x => CurrencyCode.SupportedCurrencyCode.Contains(x))
            .WithMessage($"We only support {string.Join(", " ,CurrencyCode.SupportedCurrencyCode)}")
            .WithErrorCode(ValidationErrorCodes.CurrencyNotSupported);
        RuleFor(x => x.DebtorReference).NotEmpty();
        RuleFor(x => x.DebtorName).NotEmpty();
        RuleFor(x => x.DueDate).NotEmpty();
        RuleFor(x => x.IssueDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.Date);
        RuleFor(x => x.OpeningValue).GreaterThan(0);
        RuleFor(x => x.PaidValue).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DebtorCountryCode).NotEmpty();
    }
}