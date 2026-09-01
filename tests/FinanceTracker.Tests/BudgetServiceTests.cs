using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;

namespace FinanceTracker.Tests;

public class BudgetServiceTests
{
    [Fact]
    public void CalculateTotalExpenses_ReturnsCorrectTotal()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Amount = 100000,
                Type = TransactionType.Expense
            },
            new Transaction
            {
                Id = 2,
                Amount = 50000,
                Type = TransactionType.Expense
            },
            new Transaction
            {
                Id = 3,
                Amount = 500000,
                Type = TransactionType.Income
            }
        };

        var service = new BudgetService();

        decimal result = service.CalculateTotalExpenses(transactions);

        Assert.Equal(150000, result);
    }

    [Fact]
    public void CalculateRemainingBudget_ReturnsCorrectAmount()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Amount = 150000,
                Type = TransactionType.Expense
            }
        };

        var service = new BudgetService();

        decimal result =
            service.CalculateRemainingBudget(400000, transactions);

        Assert.Equal(250000, result);
    }

    [Fact]
    public void CalculatePercentageUsed_ReturnsCorrectPercentage()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Amount = 150000,
                Type = TransactionType.Expense
            }
        };

        var service = new BudgetService();

        decimal result =
            service.CalculatePercentageUsed(400000, transactions);

        Assert.Equal(37.5m, result);
    }

    [Fact]
    public void IsBudgetExceeded_ReturnsTrueWhenExpensesExceedBudget()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Amount = 450000,
                Type = TransactionType.Expense
            }
        };

        var service = new BudgetService();

        bool result =
            service.IsBudgetExceeded(400000, transactions);

    Assert.True(result);
}

[Fact]
public void GetBudgetStatus_ReturnsWithinBudgetBelow80Percent()
{
    var transactions = new List<Transaction>
    {
        new Transaction
        {
            Id = 1,
            Amount = 150000,
            Type = TransactionType.Expense
        }
    };

    var service = new BudgetService();

    string result =
        service.GetBudgetStatus(400000, transactions);

    Assert.Equal("Within Budget", result);
}

[Fact]
public void GetBudgetStatus_ReturnsWarningAt80PercentOrMore()
{
    var transactions = new List<Transaction>
    {
        new Transaction
        {
            Id = 1,
            Amount = 320000,
            Type = TransactionType.Expense
        }
    };

    var service = new BudgetService();

    string result =
        service.GetBudgetStatus(400000, transactions);

    Assert.Equal("Warning", result);
}

[Fact]
public void GetBudgetStatus_ReturnsExceededAbove100Percent()
{
    var transactions = new List<Transaction>
    {
        new Transaction
        {
            Id = 1,
            Amount = 450000,
            Type = TransactionType.Expense
        }
    };

    var service = new BudgetService();

    string result =
        service.GetBudgetStatus(400000, transactions);

    Assert.Equal("Exceeded", result);
}
}