namespace RecSum.Domain.Invoice;

public readonly struct ExchangePair
{
    public ExchangePair(string sourceCurrency, string destinationCurrency)
    {
        SourceCurrency = sourceCurrency;
        DestinationCurrency = destinationCurrency;
    }
    public string SourceCurrency { get; init; }
    public string DestinationCurrency { get; init; }
}