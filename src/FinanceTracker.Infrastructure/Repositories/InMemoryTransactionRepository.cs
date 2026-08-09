using FinanceTracker.Core.Interfaces;
using FinanceTracker.Core.Models;

namespace FinanceTracker.Infrastructure.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly List<Transaction> _transactions = new();

    public void Add(Transaction transaction)
    {
        _transactions.Add(transaction);
    }

    public List<Transaction> GetAll()
    {
        return _transactions;
    }

    public Transaction? GetById(int id)
    {
        return _transactions.FirstOrDefault(transaction => transaction.Id == id);
    }

    public void Delete(int id)
    {
        var transaction = GetById(id);

        if (transaction != null)
        {
            _transactions.Remove(transaction);
        }
    }
}