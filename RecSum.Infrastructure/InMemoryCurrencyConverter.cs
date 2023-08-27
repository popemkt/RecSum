using RecSum.Application.Invoice;
using RecSum.Domain.Invoice;

namespace RecSum.Infrastructure;

public class InMemoryCurrencyConverter : ICurrencyConverter
{
    private readonly Dictionary<ExchangePair, double> _ratios = new()
    {
        { new ExchangePair("VND", "USD"), .5 },
        { new ExchangePair("USD", "VND"), 2 }
    };
    public double GetRatio(string sourceCurrency, string destinationCurrency)
    {
        if (sourceCurrency.Equals(destinationCurrency, StringComparison.InvariantCultureIgnoreCase))
            return 1;

        _ = _ratios.TryGetValue(new ExchangePair(sourceCurrency, destinationCurrency), out double ratio);

        return ratio;
    }
}