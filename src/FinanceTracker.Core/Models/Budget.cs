using FinanceTracker.Core.Enums;

namespace FinanceTracker.Core.Models;

public class Budget
{
    public int Year { get; set; }

    public int Month { get; set; }

    public decimal Amount { get; set; }

    public CurrencyCode Currency { get; set; }
}