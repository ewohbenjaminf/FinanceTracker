using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Services;

namespace FinanceTracker.Tests;

public class CurrencyConversionServiceTests
{
    [Fact]
    public void Convert_ShouldConvertUsdToNgn()
    {
        var service = new CurrencyConversionService();

        decimal result = service.Convert(
            100,
            CurrencyCode.USD,
            CurrencyCode.NGN,
            1500);

        Assert.Equal(150000, result);
    }

    [Fact]
    public void Convert_ShouldConvertEurToGbp()
    {
        var service = new CurrencyConversionService();

        decimal result = service.Convert(
            100,
            CurrencyCode.EUR,
            CurrencyCode.GBP,
            0.86m);

        Assert.Equal(86, result);
    }

    [Fact]
    public void Convert_SameCurrency_ShouldReturnOriginalAmount()
    {
        var service = new CurrencyConversionService();

        decimal result = service.Convert(
            100,
            CurrencyCode.USD,
            CurrencyCode.USD,
            1);

        Assert.Equal(100, result);
    }

    [Fact]
    public void Convert_NegativeAmount_ShouldThrowException()
    {
        var service = new CurrencyConversionService();

        Assert.Throws<ArgumentException>(() =>
            service.Convert(
                -100,
                CurrencyCode.USD,
                CurrencyCode.NGN,
                1500));
    }

    [Fact]
    public void Convert_InvalidExchangeRate_ShouldThrowException()
    {
        var service = new CurrencyConversionService();

        Assert.Throws<ArgumentException>(() =>
            service.Convert(
                100,
                CurrencyCode.USD,
                CurrencyCode.NGN,
                0));
    }
}