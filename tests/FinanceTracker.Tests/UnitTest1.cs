using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;
using FinanceTracker.Infrastructure.Repositories;

namespace FinanceTracker.Tests;

public class TransactionRepositoryTests
{
    [Fact]
    public void Add_ShouldStoreTransaction()
    {
        var repository = new InMemoryTransactionRepository();

        var transaction = new Transaction
        {
            Id = 1,
            Amount = 5000,
            Description = "Salary",
            Date = new DateTime(2026, 8, 9),
            Type = TransactionType.Income
        };

        repository.Add(transaction);

        var transactions = repository.GetAll();

        Assert.Single(transactions);
        Assert.Equal(transaction, transactions[0]);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectTransaction()
    {
        var repository = new InMemoryTransactionRepository();

        var transaction = new Transaction
        {
            Id = 2,
            Amount = 1500,
            Description = "Transportation",
            Date = new DateTime(2026, 8, 9),
            Type = TransactionType.Expense
        };

        repository.Add(transaction);

        var result = repository.GetById(2);

        Assert.NotNull(result);
        Assert.Equal("Transportation", result.Description);
        Assert.Equal(1500, result.Amount);
    }

    [Fact]
    public void Delete_ShouldRemoveTransaction()
    {
        var repository = new InMemoryTransactionRepository();

        var transaction = new Transaction
        {
            Id = 3,
            Amount = 2500,
            Description = "Food",
            Date = new DateTime(2026, 8, 9),
            Type = TransactionType.Expense
        };

        repository.Add(transaction);
        repository.Delete(3);

        var result = repository.GetById(3);

        Assert.Null(result);
    }
}