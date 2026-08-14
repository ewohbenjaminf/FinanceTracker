using FinanceTracker.Core.Enums;

namespace FinanceTracker.Core.Services;

public class CurrencyConversionService
{
    public decimal Convert(
        decimal amount,
        CurrencyCode fromCurrency,
        CurrencyCode toCurrency,
        decimal exchangeRate)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative.");
        }

        if (exchangeRate <= 0)
        {
            throw new ArgumentException("Exchange rate must be greater than zero.");
        }

        if (fromCurrency == toCurrency)
        {
            return amount;
        }

        return amount * exchangeRate;
    }
}