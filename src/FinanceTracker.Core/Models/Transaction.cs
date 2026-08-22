using FinanceTracker.Core.Enums;

namespace FinanceTracker.Core.Models;

public class Transaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public TransactionType Type { get; set; }

    public CurrencyCode Currency { get; set; }

    public TransactionCategory Category { get; set; }
}