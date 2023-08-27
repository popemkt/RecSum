namespace RecSum.Domain.Configurations;

public class SummaryConfiguration
{
    public int MonthsToConsider { get; set; } = 9;
    public string DefaultCurrencyCode { get; set; } = "VND";
}