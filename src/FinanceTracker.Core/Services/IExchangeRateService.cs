namespace FinanceTracker.Core.Services;

public interface IExchangeRateService
{
    Task<decimal> GetExchangeRateAsync(
        string fromCurrency,
        string toCurrency);
}