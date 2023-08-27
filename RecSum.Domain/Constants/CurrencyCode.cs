using System.Collections.ObjectModel;

namespace RecSum.Domain.Constants;

public static class CurrencyCode
{
    public const string VND = "VND";
    public const string USD = "USD";
    public static readonly ReadOnlyCollection<string> SupportedCurrencyCode = new List<string>(){ VND, USD }.AsReadOnly();
}