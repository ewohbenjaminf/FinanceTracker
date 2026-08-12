using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Services;

public class TransactionService
{
    private readonly List<Transaction> _transactions = new();

    public void AddTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
    }

    public List<Transaction> GetAllTransactions()
    {
        return _transactions;
    }

    public Transaction? GetTransactionById(int id)
    {
        return _transactions.FirstOrDefault(t => t.Id == id);
    }

    public bool DeleteTransaction(int id)
    {
        var transaction = GetTransactionById(id);

        if (transaction == null)
        {
            return false;
        }

        _transactions.Remove(transaction);
        return true;
    }
}