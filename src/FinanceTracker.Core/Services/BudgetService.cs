using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Services;

public class BudgetService
{
    public decimal CalculateTotalExpenses(List<Transaction> transactions)
    {
        return transactions
            .Where(transaction => transaction.Type == FinanceTracker.Core.Enums.TransactionType.Expense)
            .Sum(transaction => transaction.Amount);
    }

    public decimal CalculateRemainingBudget(
        decimal budget,
        List<Transaction> transactions)
    {
        decimal expenses = CalculateTotalExpenses(transactions);

        return budget - expenses;
    }

    public decimal CalculatePercentageUsed(
        decimal budget,
        List<Transaction> transactions)
    {
        if (budget <= 0)
        {
            return 0;
        }

        decimal expenses = CalculateTotalExpenses(transactions);

        return (expenses / budget) * 100;
    }

    public bool IsBudgetExceeded(
        decimal budget,
        List<Transaction> transactions)
    {
        decimal expenses = CalculateTotalExpenses(transactions);

        return expenses > budget;
    }
}