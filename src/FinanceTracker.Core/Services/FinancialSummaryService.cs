using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Services;

public class FinancialSummaryService
{
    public decimal CalculateTotalIncome(List<Transaction> transactions)
    {
        return transactions
            .Where(transaction => transaction.Type == TransactionType.Income)
            .Sum(transaction => transaction.Amount);
    }

    public decimal CalculateTotalExpenses(List<Transaction> transactions)
    {
        return transactions
            .Where(transaction => transaction.Type == TransactionType.Expense)
            .Sum(transaction => transaction.Amount);
    }

    public decimal CalculateBalance(List<Transaction> transactions)
    {
        decimal income = CalculateTotalIncome(transactions);
        decimal expenses = CalculateTotalExpenses(transactions);

        return income - expenses;
    }
}