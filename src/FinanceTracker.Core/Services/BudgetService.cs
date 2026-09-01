using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Services;

public class BudgetService
{
    public decimal CalculateTotalExpenses(List<Transaction> transactions)
    {
        return transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);
    }

    public decimal CalculateRemainingBudget(
        decimal budgetAmount,
        List<Transaction> transactions)
    {
        decimal totalExpenses = CalculateTotalExpenses(transactions);

        return budgetAmount - totalExpenses;
    }

    public decimal CalculatePercentageUsed(
        decimal budgetAmount,
        List<Transaction> transactions)
    {
        if (budgetAmount <= 0)
        {
            return 0;
        }

        decimal totalExpenses = CalculateTotalExpenses(transactions);

        return (totalExpenses / budgetAmount) * 100;
    }

    public bool IsBudgetExceeded(
        decimal budgetAmount,
        List<Transaction> transactions)
    {
        decimal totalExpenses = CalculateTotalExpenses(transactions);

        return totalExpenses > budgetAmount;
    }
}