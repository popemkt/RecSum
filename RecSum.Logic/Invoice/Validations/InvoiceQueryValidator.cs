using FluentValidation;
using RecSum.Domain.Invoice;

namespace RecSum.Application.Invoice.Validations;

public class InvoiceQueryValidator : AbstractValidator<InvoiceSummaryQuery>
{
    public InvoiceQueryValidator()
    {
        RuleFor(x => x.Currency);
        RuleFor(x => x.MonthsToConsider).InclusiveBetween(6, 48)
            .When(x => x.MonthsToConsider.HasValue);
    }
}