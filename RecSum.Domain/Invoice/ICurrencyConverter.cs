namespace RecSum.Domain.Invoice;

public interface ICurrencyConverter
{
    double GetRatio(string sourceCurrency, string destinationCurrency);
}