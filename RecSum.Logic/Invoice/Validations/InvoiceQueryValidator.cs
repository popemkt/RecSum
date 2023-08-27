using FluentValidation;
using RecSum.Domain.Constants;
using RecSum.Domain.Errors;
using RecSum.Domain.Invoice;

namespace RecSum.Application.Invoice.Validations;

public class InvoiceQueryValidator : AbstractValidator<InvoiceSummaryQuery>
{
    public InvoiceQueryValidator()
    {
        RuleFor(x => x.Currency)
            .Must(x => CurrencyCode.SupportedCurrencyCode.Contains(x))
            .WithMessage($"We only support {string.Join(", " ,CurrencyCode.SupportedCurrencyCode)}")
            .WithErrorCode(ValidationErrorCodes.CurrencyNotSupported)
            .When(x => x.Currency is not null);
        
        RuleFor(x => x.MonthsToConsider).InclusiveBetween(6, 48)
            .When(x => x.MonthsToConsider.HasValue);
    }
}