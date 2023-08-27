namespace RecSum.Application.Invoice;

public interface ICurrencyConverter
{
    double GetRatio(string sourceCurrency, string destinationCurrency);
}