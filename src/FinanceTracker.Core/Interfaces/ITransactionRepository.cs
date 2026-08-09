using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Interfaces;

public interface ITransactionRepository
{
    void Add(Transaction transaction);

    List<Transaction> GetAll();

    Transaction? GetById(int id);

    void Delete(int id);
}