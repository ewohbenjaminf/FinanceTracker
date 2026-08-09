using FinanceTracker.Core.Enums;

namespace FinanceTracker.Core.Models;

public class Transaction
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public string Category { get; set; } = string.Empty;
}

using FinanceTracker.Core.Enums;

namespace FinanceTracker.Core.Models;

public class Transaction
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public string Category { get; set; } = string.Empty;
}