using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;

namespace FinanceTracker.Tests;

public class TransactionServiceTests
{
    [Fact]
    public void AddTransaction_ShouldStoreTransaction()
    {
        var service = new TransactionService();

var transaction = new Transaction
{
    Id = 1,
    Amount = 5000,
    Description = "Salary",
    Date = new DateTime(2026, 8, 9),
    Type = TransactionType.Income,
    Currency = CurrencyCode.NGN,
    Category = TransactionCategory.Salary
};

        service.AddTransaction(transaction);

        var transactions = service.GetAllTransactions();

        Assert.Single(transactions);
        Assert.Equal(transaction, transactions[0]);
    }

    [Fact]
    public void GetTransactionById_ShouldReturnCorrectTransaction()
    {
        var service = new TransactionService();

    var transaction = new Transaction
{
    Id = 2,
    Amount = 1500,
    Description = "Transportation",
    Date = new DateTime(2026, 8, 9),
    Type = TransactionType.Expense,
    Currency = CurrencyCode.NGN,
    Category = TransactionCategory.Transportation
};

        service.AddTransaction(transaction);

        var result = service.GetTransactionById(2);

        Assert.NotNull(result);
        Assert.Equal("Transportation", result.Description);
        Assert.Equal(1500, result.Amount);
    }

    [Fact]
    public void DeleteTransaction_ShouldRemoveTransaction()
    {
        var service = new TransactionService();

        var transaction = new Transaction
{
    Id = 3,
    Amount = 2500,
    Description = "Food",
    Date = new DateTime(2026, 8, 9),
    Type = TransactionType.Expense,
    Currency = CurrencyCode.NGN,
    Category = TransactionCategory.Food
};

        service.AddTransaction(transaction);

        var deleted = service.DeleteTransaction(3);

        var result = service.GetTransactionById(3);

        Assert.True(deleted);
        Assert.Null(result);
    }
}

