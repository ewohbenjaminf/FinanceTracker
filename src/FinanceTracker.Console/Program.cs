using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using FinanceTracker.Infrastructure.Repositories;

var repository = new InMemoryTransactionRepository();
var summaryService = new FinancialSummaryService();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("================================");
    Console.WriteLine("        FINANCE TRACKER");
    Console.WriteLine("================================");
    Console.WriteLine("1. Add Transaction");
    Console.WriteLine("2. View Transactions");
    Console.WriteLine("3. Find Transaction");
    Console.WriteLine("4. Delete Transaction");
Console.WriteLine("5. Financial Summary");
Console.WriteLine("6. Exit");
    Console.WriteLine("================================");
    Console.Write("Select an option: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddTransaction();
            break;

        case "2":
            ViewTransactions();
            break;

        case "3":
            FindTransaction();
            break;

        case "4":
            DeleteTransaction();
            break;

Console.WriteLine("4. Delete Transaction");
Console.WriteLine("5. Financial Summary");
Console.WriteLine("6. Exit");

        default:
            Console.WriteLine("Invalid option.");
            Pause();
            break;
    }
}

void AddTransaction()
{
    Console.WriteLine();
    Console.WriteLine("=== Add Transaction ===");

    Console.Write("Enter transaction ID: ");
    int id = int.Parse(Console.ReadLine()!);

    Console.Write("Enter amount: ");
    decimal amount = decimal.Parse(Console.ReadLine()!);

    Console.Write("Enter description: ");
    string description = Console.ReadLine()!;

    Console.Write("Enter date (yyyy-MM-dd): ");
    DateTime date = DateTime.Parse(Console.ReadLine()!);

    Console.Write("Enter type (Income/Expense): ");
    string typeInput = Console.ReadLine()!;

    TransactionType type = Enum.Parse<TransactionType>(typeInput, true);

    var transaction = new Transaction
    {
        Id = id,
        Amount = amount,
        Description = description,
        Date = date,
        Type = type
    };

    repository.Add(transaction);

    Console.WriteLine();
    Console.WriteLine("Transaction added successfully.");
    Pause();
}

void ViewTransactions()
{
    Console.WriteLine();
    Console.WriteLine("=== All Transactions ===");

    var transactions = repository.GetAll();

    if (transactions.Count == 0)
    {
        Console.WriteLine("No transactions found.");
        Pause();
        return;
    }

    foreach (var transaction in transactions)
    {
        Console.WriteLine(
            $"ID: {transaction.Id} | " +
            $"Amount: {transaction.Amount:C} | " +
            $"Description: {transaction.Description} | " +
            $"Date: {transaction.Date:yyyy-MM-dd} | " +
            $"Type: {transaction.Type}");
    }

    Pause();
}

void FindTransaction()
{
    Console.WriteLine();
    Console.WriteLine("=== Find Transaction ===");

    Console.Write("Enter transaction ID: ");
    int id = int.Parse(Console.ReadLine()!);

    var transaction = repository.GetById(id);

    if (transaction == null)
    {
        Console.WriteLine("Transaction not found.");
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine($"ID: {transaction.Id}");
        Console.WriteLine($"Amount: {transaction.Amount:C}");
        Console.WriteLine($"Description: {transaction.Description}");
        Console.WriteLine($"Date: {transaction.Date:yyyy-MM-dd}");
        Console.WriteLine($"Type: {transaction.Type}");
    }

    Pause();
}

void DeleteTransaction()
{
    Console.WriteLine();
    Console.WriteLine("=== Delete Transaction ===");

    Console.Write("Enter transaction ID: ");
    int id = int.Parse(Console.ReadLine()!);

    var transaction = repository.GetById(id);

    if (transaction == null)
    {
        Console.WriteLine("Transaction not found.");
    }
    else
    {
        repository.Delete(id);
        Console.WriteLine("Transaction deleted successfully.");
    }

    Pause();
}

void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
}

void ShowFinancialSummary()
{
    Console.WriteLine();
    Console.WriteLine("=== Financial Summary ===");

    var transactions = repository.GetAll();

    decimal totalIncome = summaryService.CalculateTotalIncome(transactions);
    decimal totalExpenses = summaryService.CalculateTotalExpenses(transactions);
    decimal balance = summaryService.CalculateBalance(transactions);

    Console.WriteLine($"Total Income:   {totalIncome:C}");
    Console.WriteLine($"Total Expenses: {totalExpenses:C}");
    Console.WriteLine($"Balance:        {balance:C}");

    Pause();
}