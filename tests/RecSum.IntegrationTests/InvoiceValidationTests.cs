using Bogus;
using RecSum.Application.Invoice.Validations;
using RecSum.Domain.Constants;
using RecSum.Domain.Errors;
using RecSum.Domain.Invoice;
using RecSum.Domain.Invoice.Dtos;
using Shouldly;

namespace RecSum.IntegrationTests;

public class InvoiceValidationTests
{
    private readonly Faker _faker;

    public InvoiceValidationTests()
    {
        _faker = new Faker("en");
    }
    [Fact]
    public async Task InvoiceImport_SufficientInfo_ShouldSucceed()
    {
        var dto = new ImportInvoiceDto()
        {
            Reference = Guid.NewGuid().ToString(),
            CurrencyCode = CurrencyCode.VND,
            DebtorReference = Guid.NewGuid().ToString(),
            DebtorName = _faker.Name.FullName(),
            IssueDate = DateTime.UtcNow.Date,
            OpeningValue = 1000,
            PaidValue = 0,
            DebtorCountryCode = _faker.Address.CountryCode(),
            DueDate = DateTime.UtcNow.Date.AddMonths(1)
        };
        var importInvoiceCommand = new ImportInvoiceCommand().WithDtos(new List<ImportInvoiceDto>()
        {
            dto
        });
        var validator = new ImportInvoiceValidator();


        var result = await validator.ValidateAsync(importInvoiceCommand);
        result.IsValid.ShouldBeTrue();
    }
    
    
    [Fact]
    public async Task InvoiceImport_Empty_ShouldFail()
    {
        var importInvoiceCommand = new ImportInvoiceCommand().WithDtos(new List<ImportInvoiceDto>());
        
        var validator = new ImportInvoiceValidator();

        var result = await validator.ValidateAsync(importInvoiceCommand);
        result.IsValid.ShouldBeFalse();
    }
    
    [Fact]
    public async Task InvoiceImport_InvalidConstraints_ShouldFail()
    {
        var dto = new ImportInvoiceDto()
        {
        };
        
        var importInvoiceCommand = new ImportInvoiceCommand().WithDtos(new List<ImportInvoiceDto>()
        {
            dto
        });
        var validator = new ImportInvoiceValidator();

        var result = await validator.ValidateAsync(importInvoiceCommand);
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBeEquivalentTo(9);
        result.Errors.ShouldContain(x => x.ErrorCode == "NotEmptyValidator" && x.PropertyName.Contains(nameof(dto.Reference)));
        result.Errors.ShouldContain(x => x.ErrorCode == "NotEmptyValidator" && x.PropertyName.Contains(nameof(dto.DebtorReference)));
        result.Errors.ShouldContain(x => x.ErrorCode == "NotEmptyValidator" && x.PropertyName.Contains(nameof(dto.CurrencyCode)));
        result.Errors.ShouldContain(x => x.ErrorCode == ValidationErrorCodes.CurrencyNotSupported && x.PropertyName.Contains(nameof(dto.CurrencyCode)));
        result.Errors.ShouldContain(x => x.ErrorCode == "NotEmptyValidator" && x.PropertyName.Contains(nameof(dto.DebtorName)));
        result.Errors.ShouldContain(x => x.ErrorCode == "NotEmptyValidator" && x.PropertyName.Contains(nameof(dto.DueDate)));
        result.Errors.ShouldContain(x => x.ErrorCode == "NotEmptyValidator" && x.PropertyName.Contains(nameof(dto.IssueDate)));
        result.Errors.ShouldContain(x => x.ErrorCode == "GreaterThanValidator" && x.PropertyName.Contains(nameof(dto.OpeningValue)));
        result.Errors.ShouldContain(x => x.ErrorCode == "NotEmptyValidator" && x.PropertyName.Contains(nameof(dto.DebtorCountryCode)));

        dto.Reference = Guid.NewGuid().ToString();
        dto.DebtorReference = Guid.NewGuid().ToString();
        dto.CurrencyCode = CurrencyCode.VND;
        dto.DebtorName = _faker.Name.FullName();
        dto.DebtorCountryCode = _faker.Address.CountryCode();
        dto.IssueDate = DateTime.UtcNow.Date.AddDays(1);
        dto.DueDate = dto.IssueDate.AddMonths(3);
        dto.Cancelled = true;
        dto.OpeningValue = 10000;
        dto.DebtorAddress1 = _faker.Address.FullAddress();
        dto.DebtorAddress2 = _faker.Address.FullAddress();
        dto.DebtorTown = _faker.Address.City();
        dto.DebtorZip = _faker.Address.ZipCode();
        dto.DebtorState = _faker.Address.State();
        dto.DebtorRegistrationNumber = _faker.Finance.Bic();

        result = await validator.ValidateAsync(importInvoiceCommand);
        result.Errors.Count.ShouldBeEquivalentTo(1);
        result.Errors.ShouldContain(x => x.ErrorCode == "LessThanOrEqualValidator" && x.PropertyName.Contains(nameof(dto.IssueDate)));

        dto.IssueDate = DateTime.UtcNow.Date;
        result = await validator.ValidateAsync(importInvoiceCommand);
        result.IsValid.ShouldBeTrue();
    }
    
    [Fact]
    public async Task InvoiceQuery_ShouldSucceed()
    {
        var query = new InvoiceSummaryQuery()
        {
        };
        var validator = new InvoiceQueryValidator();

        var result = await validator.ValidateAsync(query);
        result.IsValid.ShouldBeTrue();

        query.MonthsToConsider = 12;
        query.Currency = CurrencyCode.VND;
        
        result = await validator.ValidateAsync(query);
        result.IsValid.ShouldBeTrue();
    }
    
    [Fact]
    public async Task InvoiceQuery_InvalidData_ShouldFail()
    {
        var query = new InvoiceSummaryQuery()
        {
            MonthsToConsider = -10,
            Currency = "RANDOM",
        };
        
        var validator = new InvoiceQueryValidator();
        
        var result = await validator.ValidateAsync(query);
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBeEquivalentTo(2);
        result.Errors.ShouldContain(x => x.ErrorCode == "InclusiveBetweenValidator" && x.PropertyName.Contains(nameof(query.MonthsToConsider)));
        result.Errors.ShouldContain(x => x.ErrorCode == ValidationErrorCodes.CurrencyNotSupported && x.PropertyName.Contains(nameof(query.Currency)));
    }
}