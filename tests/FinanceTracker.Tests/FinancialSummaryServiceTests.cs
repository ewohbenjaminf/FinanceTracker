using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;

namespace FinanceTracker.Tests;

public class FinancialSummaryServiceTests
{
    [Fact]
    public void CalculateTotalIncome_ShouldReturnCorrectTotal()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Amount = 50000,
                Description = "Salary",
                Type = TransactionType.Income
            },
            new Transaction
            {
                Id = 2,
                Amount = 20000,
                Description = "Freelance",
                Type = TransactionType.Income
            }
        };

        var service = new FinancialSummaryService();

        var result = service.CalculateTotalIncome(transactions);

        Assert.Equal(70000, result);
    }

    [Fact]
    public void CalculateTotalExpenses_ShouldReturnCorrectTotal()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Amount = 10000,
                Description = "Food",
                Type = TransactionType.Expense
            },
            new Transaction
            {
                Id = 2,
                Amount = 5000,
                Description = "Transport",
                Type = TransactionType.Expense
            }
        };

        var service = new FinancialSummaryService();

        var result = service.CalculateTotalExpenses(transactions);

        Assert.Equal(15000, result);
    }

    [Fact]
    public void CalculateBalance_ShouldReturnIncomeMinusExpenses()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Amount = 50000,
                Description = "Salary",
                Type = TransactionType.Income
            },
            new Transaction
            {
                Id = 2,
                Amount = 15000,
                Description = "Rent",
                Type = TransactionType.Expense
            }
        };

        var service = new FinancialSummaryService();

        var result = service.CalculateBalance(transactions);

        Assert.Equal(35000, result);
    }
}