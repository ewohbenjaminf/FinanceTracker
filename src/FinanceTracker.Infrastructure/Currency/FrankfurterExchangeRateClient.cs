using System.Net.Http.Json;
using FinanceTracker.Core.Services;

namespace FinanceTracker.Infrastructure.Currency;

public class FrankfurterExchangeRateClient : IExchangeRateService
{
    private readonly HttpClient _httpClient;

    public FrankfurterExchangeRateClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetExchangeRateAsync(
        string fromCurrency,
        string toCurrency)
    {
        string url =
            $"v2/rate/{fromCurrency.ToUpperInvariant()}/{toCurrency.ToUpperInvariant()}";

        var response =
            await _httpClient.GetFromJsonAsync<FrankfurterRateResponse>(url);

        if (response is null)
        {
            throw new InvalidOperationException(
                "Unable to retrieve the exchange rate.");
        }

        return response.Rate;
    }

    private sealed class FrankfurterRateResponse
    {
        public decimal Rate { get; set; }
    }
}