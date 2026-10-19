using System.Text.Json;
using FinanceTracker.Core.Interfaces;
using FinanceTracker.Core.Models;

namespace FinanceTracker.Infrastructure.Repositories;

public class JsonTransactionRepository : ITransactionRepository
{
    private readonly string _filePath;

    public JsonTransactionRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(Transaction transaction)
    {
        var transactions = GetAll();
        transactions.Add(transaction);
        Save(transactions);
    }

    public List<Transaction> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Transaction>();
        }

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Transaction>();
        }

        return JsonSerializer.Deserialize<List<Transaction>>(json)
               ?? new List<Transaction>();
    }

    public Transaction? GetById(int id)
    {
        return GetAll().FirstOrDefault(t => t.Id == id);
    }

public void Update(Transaction transaction)
{
    var transactions = GetAll();

    int index = transactions.FindIndex(t => t.Id == transaction.Id);

    if (index == -1)
    {
        throw new InvalidOperationException(
            $"Transaction with ID {transaction.Id} was not found.");
    }

    transactions[index] = transaction;

    Save(transactions);
}
    public void Delete(int id)
    {
        var transactions = GetAll();

        var transaction = transactions.FirstOrDefault(t => t.Id == id);

        if (transaction != null)
        {
            transactions.Remove(transaction);
            Save(transactions);
        }
    }

    private void Save(List<Transaction> transactions)
    {
        string? directory = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonSerializer.Serialize(
            transactions,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(_filePath, json);
    }
}