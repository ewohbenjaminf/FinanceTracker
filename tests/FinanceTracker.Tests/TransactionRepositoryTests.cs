
using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;
using FinanceTracker.Infrastructure.Repositories;

namespace FinanceTracker.Tests;

public class TransactionRepositoryUpdateTests
{
    [Fact]
    public void InMemoryRepository_Update_ShouldReplaceExistingTransaction()
    {
        var repository = new InMemoryTransactionRepository();

        var original = CreateTransaction(1, "Original description");
        repository.Add(original);

        var updated = CreateTransaction(1, "Updated description");
        repository.Update(updated);

        var result = repository.GetById(1);

        Assert.NotNull(result);
        Assert.Equal("Updated description", result.Description);
        Assert.Single(repository.GetAll());
    }

    [Fact]
    public void InMemoryRepository_Update_ShouldThrowWhenTransactionDoesNotExist()
    {
        var repository = new InMemoryTransactionRepository();
        var transaction = CreateTransaction(99, "Missing transaction");

        Assert.Throws<InvalidOperationException>(
            () => repository.Update(transaction));
    }

    [Fact]
    public void JsonRepository_Update_ShouldSaveChangesToFile()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"FinanceTrackerTests_{Guid.NewGuid():N}.json");

        try
        {
            var repository = new JsonTransactionRepository(filePath);
            repository.Add(CreateTransaction(1, "Original description"));

            repository.Update(CreateTransaction(1, "Updated description"));

            // Create a new repository instance to verify saved data.
            var reloadedRepository = new JsonTransactionRepository(filePath);
            var result = reloadedRepository.GetById(1);

            Assert.NotNull(result);
            Assert.Equal("Updated description", result.Description);
            Assert.Single(reloadedRepository.GetAll());
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void JsonRepository_Update_ShouldThrowWhenTransactionDoesNotExist()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"FinanceTrackerTests_{Guid.NewGuid():N}.json");

        try
        {
            var repository = new JsonTransactionRepository(filePath);
            var transaction = CreateTransaction(99, "Missing transaction");

            Assert.Throws<InvalidOperationException>(
                () => repository.Update(transaction));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    private static Transaction CreateTransaction(int id, string description)
    {
        return new Transaction
        {
            Id = id,
            Amount = 100,
            Description = description,
            Date = new DateTime(2026, 10, 19),
            Type = TransactionType.Income,
            Currency = CurrencyCode.NGN,
            Category = TransactionCategory.Salary
        };
    }
}
