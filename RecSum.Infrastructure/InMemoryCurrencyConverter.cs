using Microsoft.Extensions.Logging;
using RecSum.Domain.Constants;
using RecSum.Domain.Invoice;

namespace RecSum.Infrastructure;

public class InMemoryCurrencyConverter : ICurrencyConverter
{
    private readonly ILogger<InMemoryCurrencyConverter> _logger;

    public InMemoryCurrencyConverter(ILogger<InMemoryCurrencyConverter> logger)
    {
        _logger = logger;
    }
    private readonly Dictionary<ExchangePair, double> _ratios = new()
    {
        { new ExchangePair(CurrencyCode.VND, CurrencyCode.USD), .5 },
        { new ExchangePair(CurrencyCode.USD, CurrencyCode.VND), 2 }
    };
    public double GetRatio(string sourceCurrency, string destinationCurrency)
    {
        if (sourceCurrency.Equals(destinationCurrency, StringComparison.InvariantCultureIgnoreCase))
            return 1;

        var convertSucceeded = _ratios.TryGetValue(new ExchangePair(sourceCurrency, destinationCurrency), out double ratio);

        if (!convertSucceeded)
        {
            _logger.LogError("Currency pair of {sourceCurrency} to {destinationCurrency} not supported", sourceCurrency, destinationCurrency);
            throw new ArgumentException("Currency pair exchange not supported");
        }
        
        return ratio;
    }
}