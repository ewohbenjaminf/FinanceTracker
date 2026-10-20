using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using FinanceTracker.Infrastructure.Currency;
using FinanceTracker.Infrastructure.Repositories;

var repository = new JsonTransactionRepository(
    Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "FinanceTracker",
        "transactions.json"));
var summaryService = new FinancialSummaryService();

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://api.frankfurter.dev/")
};

var exchangeRateService =
    new FrankfurterExchangeRateClient(httpClient);
while (true)
{
    Console.WriteLine();
    Console.WriteLine("================================");
    Console.WriteLine("        FINANCE TRACKER");
    Console.WriteLine("================================");

    Console.WriteLine("1. Add Transaction");
    Console.WriteLine("2. View Transactions");
    Console.WriteLine("3. Find Transaction");
    Console.WriteLine("4. Edit Transaction");
    Console.WriteLine("5. Delete Transaction");
    Console.WriteLine("6. Financial Summary");
    Console.WriteLine("7. Monthly Summary");
    Console.WriteLine("8. Currency Conversion");
    Console.WriteLine("9. Budget Management");
    Console.WriteLine("10. Exit");
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
            EditTransaction();
            break;

        case "5":
            DeleteTransaction();
            break;

        case "6":
            ShowFinancialSummary();
            break;

        case "7":
            ShowMonthlySummary();
            break;

        case "8":
            await ConvertCurrency();
            break;

        case "9":
            ShowBudgetManagement();
            break;

        case "10":
            Console.WriteLine("Goodbye!");
            return;

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

    if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
    {
        Console.WriteLine("Invalid transaction ID. Enter a positive whole number.");
        Pause();
        return;
    }

    Console.Write("Enter amount: ");

    if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
    {
        Console.WriteLine("Invalid amount. Enter a number greater than zero.");
        Pause();
        return;
    }

    Console.Write("Enter description: ");
    string description = Console.ReadLine()?.Trim() ?? "";

    if (string.IsNullOrWhiteSpace(description))
    {
        Console.WriteLine("Description cannot be empty.");
        Pause();
        return;
    }

    Console.Write("Enter date (yyyy-MM-dd): ");

    if (!DateTime.TryParseExact(
        Console.ReadLine(),
        "yyyy-MM-dd",
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None,
        out DateTime date))
    {
        Console.WriteLine("Invalid date. Use the format yyyy-MM-dd, for example 2026-10-20.");
        Pause();
        return;
    }

    Console.Write("Enter type (Income/Expense): ");
    string typeInput = Console.ReadLine()?.Trim() ?? "";

    if (!Enum.TryParse<TransactionType>(typeInput, true, out var type)
        || !Enum.IsDefined(type))
    {
        Console.WriteLine("Invalid transaction type. Enter Income or Expense.");
        Pause();
        return;
    }

    Console.Write("Enter currency (NGN, USD, GBP, EUR): ");
    string currencyInput = Console.ReadLine()?.Trim() ?? "";

    if (!Enum.TryParse<CurrencyCode>(
            currencyInput,
            true,
            out var currency)
        || !Enum.IsDefined(currency))
    {
        Console.WriteLine("Invalid currency. Enter NGN, USD, GBP, or EUR.");
        Pause();
        return;
    }

    Console.Write("Enter category: ");
    string categoryInput = Console.ReadLine()?.Trim() ?? "";

    if (!Enum.TryParse<TransactionCategory>(
            categoryInput,
            true,
            out var category)
        || !Enum.IsDefined(category))
    {
        Console.WriteLine("Invalid category. Please enter a valid transaction category.");
        Pause();
        return;
    }

    var transaction = new Transaction
    {
        Id = id,
        Amount = amount,
        Description = description,
        Date = date,
        Type = type,
        Currency = currency,
        Category = category
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
           $"Amount: {FormatTransactionAmount(transaction.Amount, transaction.Currency)} | " +
            $"Description: {transaction.Description} | " +
            $"Date: {transaction.Date:yyyy-MM-dd} | " +
            $"Type: {transaction.Type} | " +
            $"Category: {transaction.Category}");
    }

    Pause();
}

void FindTransaction()
{
    Console.WriteLine();
    Console.WriteLine("=== Find Transaction ===");

    Console.Write("Enter transaction ID: ");

    if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
    {
        Console.WriteLine("Invalid transaction ID. Enter a positive whole number.");
        Pause();
        return;
    }

    var transaction = repository.GetById(id);

    if (transaction == null)
    {
        Console.WriteLine("Transaction not found.");
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine($"ID: {transaction.Id}");

        Console.WriteLine(
            $"Amount: {FormatTransactionAmount(transaction.Amount, transaction.Currency)}");
        Console.WriteLine($"Description: {transaction.Description}");
        Console.WriteLine($"Date: {transaction.Date:yyyy-MM-dd}");
        Console.WriteLine($"Type: {transaction.Type}");
        Console.WriteLine($"Category: {transaction.Category}");
    }

    Pause();
}


void EditTransaction()
{
    Console.WriteLine();
    Console.WriteLine("=== Edit Transaction ===");

    Console.Write("Enter transaction ID to edit: ");

    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid transaction ID.");
        Pause();
        return;
    }

    var transaction = repository.GetById(id);

    if (transaction == null)
    {
        Console.WriteLine("Transaction not found.");
        Pause();
        return;
    }

    Console.WriteLine();
    Console.WriteLine("Press Enter to keep the current value.");

    Console.Write($"Amount ({transaction.Amount}): ");
    string amountInput = Console.ReadLine() ?? "";

    decimal amount = transaction.Amount;

    if (!string.IsNullOrWhiteSpace(amountInput))
    {
        if (!decimal.TryParse(amountInput, out amount) || amount <= 0)
        {
            Console.WriteLine("Invalid amount. The transaction was not updated.");
            Pause();
            return;
        }
    }

    Console.Write($"Description ({transaction.Description}): ");
    string descriptionInput = Console.ReadLine() ?? "";

    string description = string.IsNullOrWhiteSpace(descriptionInput)
        ? transaction.Description
        : descriptionInput.Trim();

    Console.Write($"Date ({transaction.Date:yyyy-MM-dd}): ");
    string dateInput = Console.ReadLine() ?? "";

    DateTime date = transaction.Date;

    if (!string.IsNullOrWhiteSpace(dateInput))
    {
        if (!DateTime.TryParseExact(
            dateInput,
            "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out date))
        {
            Console.WriteLine("Invalid date. Use yyyy-MM-dd. The transaction was not updated.");
            Pause();
            return;
        }
    }

    Console.Write($"Type ({transaction.Type}) - Income/Expense: ");
    string typeInput = Console.ReadLine() ?? "";

    TransactionType type = transaction.Type;

    if (!string.IsNullOrWhiteSpace(typeInput))
    {
        if (!Enum.TryParse(typeInput, true, out type)
            || !Enum.IsDefined(type))
        {
            Console.WriteLine("Invalid transaction type. The transaction was not updated.");
            Pause();
            return;
        }
    }

    Console.Write($"Currency ({transaction.Currency}): ");
    string currencyInput = Console.ReadLine() ?? "";

    CurrencyCode currency = transaction.Currency;

    if (!string.IsNullOrWhiteSpace(currencyInput))
    {
        if (!Enum.TryParse(currencyInput, true, out currency)
            || !Enum.IsDefined(currency))
        {
            Console.WriteLine("Invalid currency. The transaction was not updated.");
            Pause();
            return;
        }
    }

    Console.Write($"Category ({transaction.Category}): ");
    string categoryInput = Console.ReadLine() ?? "";

    TransactionCategory category = transaction.Category;

    if (!string.IsNullOrWhiteSpace(categoryInput))
    {
        if (!Enum.TryParse(categoryInput, true, out category)
            || !Enum.IsDefined(category))
        {
            Console.WriteLine("Invalid category. The transaction was not updated.");
            Pause();
            return;
        }
    }

    var updatedTransaction = new Transaction
    {
        Id = transaction.Id,
        Amount = amount,
        Description = description,
        Date = date,
        Type = type,
        Currency = currency,
        Category = category
    };

    repository.Update(updatedTransaction);

    Console.WriteLine();
    Console.WriteLine("Transaction updated successfully.");
    Pause();
}

void DeleteTransaction()
{
    Console.WriteLine();
    Console.WriteLine("=== Delete Transaction ===");

    Console.Write("Enter transaction ID: ");

    if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
    {
        Console.WriteLine("Invalid transaction ID. Enter a positive whole number.");
        Pause();
        return;
    }

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

    if (transactions.Count == 0)
    {
        Console.WriteLine("No transactions found.");
        Pause();
        return;
    }


    Console.WriteLine("Select currency for the summary:");

    CurrencyCode[] currencies = Enum.GetValues<CurrencyCode>();

    for (int i = 0; i < currencies.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {currencies[i]}");
    }

    Console.Write("Enter the number for your currency: ");
    string currencyInput = Console.ReadLine()?.Trim() ?? "";

    if (!int.TryParse(currencyInput, out int currencyChoice)
        || currencyChoice < 1
        || currencyChoice > currencies.Length)
    {
        Console.WriteLine($"Invalid selection. Please choose a number from 1 to {currencies.Length}.");
        Pause();
        return;
    }

    CurrencyCode selectedCurrency = currencies[currencyChoice - 1];

    var filteredTransactions = transactions
        .Where(t => t.Currency == selectedCurrency)
        .ToList();

    decimal totalIncome = summaryService.CalculateTotalIncome(filteredTransactions);
    decimal totalExpenses = summaryService.CalculateTotalExpenses(filteredTransactions);
    decimal balance = summaryService.CalculateBalance(filteredTransactions);

    Console.WriteLine();
    Console.WriteLine($"Currency:       {selectedCurrency}");
    Console.WriteLine($"Total Income:   {FormatTransactionAmount(totalIncome, selectedCurrency)}");
    Console.WriteLine($"Total Expenses: {FormatTransactionAmount(totalExpenses, selectedCurrency)}");
    Console.WriteLine($"Balance:        {FormatTransactionAmount(balance, selectedCurrency)}");

    if (filteredTransactions.Count == 0)
    {
        Console.WriteLine("No transactions found in this currency.");
    }

    Pause();
}

async Task ConvertCurrency()
{
    Console.WriteLine();
    Console.WriteLine("=== Currency Conversion ===");

    Console.Write("Enter amount: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
    {
        Console.WriteLine("Invalid amount.");
        Pause();
        return;
    }

    Console.Write("Enter source currency (e.g. NGN, USD, GBP): ");
    string fromInput = Console.ReadLine()!.Trim().ToUpperInvariant();

    Console.Write("Enter target currency (e.g. NGN, USD, GBP): ");
    string toInput = Console.ReadLine()!.Trim().ToUpperInvariant();

    if (!Enum.TryParse<CurrencyCode>(fromInput, true, out var fromCurrency))
    {
        Console.WriteLine("Invalid source currency.");
        Pause();
        return;
    }

    if (!Enum.TryParse<CurrencyCode>(toInput, true, out var toCurrency))
    {
        Console.WriteLine("Invalid target currency.");
        Pause();
        return;
    }

    try
    {
        decimal exchangeRate =
            await exchangeRateService.GetExchangeRateAsync(
                fromCurrency.ToString(),
                toCurrency.ToString());

        var conversionService = new CurrencyConversionService();

        decimal convertedAmount = conversionService.Convert(
            amount,
            fromCurrency,
            toCurrency,
            exchangeRate);

        Console.WriteLine();
        Console.WriteLine("=== Conversion Result ===");
        Console.WriteLine(
            $"{amount:N2} {fromCurrency} = {convertedAmount:N2} {toCurrency}");
        Console.WriteLine(
            $"Exchange Rate: 1 {fromCurrency} = {exchangeRate:N6} {toCurrency}");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"Currency conversion failed: {ex.Message}");
    }

    Pause();
}

void ShowMonthlySummary()
{
    Console.WriteLine();
    Console.WriteLine("=== Monthly Financial Summary ===");

    Console.Write("Enter year: ");
    int year = int.Parse(Console.ReadLine()!);

    Console.Write("Enter month (1-12): ");
    int month = int.Parse(Console.ReadLine()!);
    Console.Write("Enter currency (NGN, USD, GBP, EUR): ");
    string currencyInput = Console.ReadLine()!.ToUpper();

    var allTransactions = repository.GetAll().ToList();

    var monthlyTransactions =
        summaryService.GetTransactionsForMonth(
            allTransactions,
            year,
            month)
        .Where(transaction =>
            transaction.Currency.ToString()
                .Equals(currencyInput, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (monthlyTransactions.Count == 0)
    {
        Console.WriteLine("No transactions found for this month.");
        Pause();
        return;
    }

    decimal income =
    summaryService.CalculateTotalIncome(monthlyTransactions);

    decimal expenses =
        summaryService.CalculateTotalExpenses(monthlyTransactions);

    decimal balance =
        summaryService.CalculateBalance(monthlyTransactions);

    decimal savingsRate =
        summaryService.CalculateSavingsRate(monthlyTransactions);

    string currencySymbol = currencyInput switch
    {
        "NGN" => "₦",
        "USD" => "$",
        "GBP" => "£",
        "EUR" => "€",
        _ => currencyInput + " "
    };

    Console.WriteLine();
    Console.WriteLine($"Month: {year}-{month:D2}");
    Console.WriteLine($"Total Income:   {currencySymbol}{income:N2}");
    Console.WriteLine($"Total Expenses: {currencySymbol}{expenses:N2}");
    Console.WriteLine($"Balance:        {currencySymbol}{balance:N2}");
    Console.WriteLine($"Savings Rate:   {savingsRate:F2}%");


    Pause();
}
void ShowBudgetManagement()
{
    Console.WriteLine();
    Console.WriteLine("=== Budget Management ===");

    Console.Write("Enter monthly budget: ");

    if (!decimal.TryParse(
        Console.ReadLine(),
        out decimal budget) || budget <= 0)
    {
        Console.WriteLine("Invalid budget amount.");
        Pause();
        return;
    }

    Console.Write("Enter currency (NGN, USD, GBP, EUR): ");
    string currencyInput =
        Console.ReadLine()!.Trim().ToUpperInvariant();

    if (!Enum.TryParse<CurrencyCode>(
        currencyInput,
        true,
        out var budgetCurrency))
    {
        Console.WriteLine("Invalid currency.");
        Pause();
        return;
    }

    var transactions = repository.GetAll()
        .Where(t => t.Currency == budgetCurrency)
        .ToList();

    var budgetService = new BudgetService();

    decimal expenses =
        budgetService.CalculateTotalExpenses(transactions);

    decimal remaining =
        budgetService.CalculateRemainingBudget(
            budget,
            transactions);

    decimal percentageUsed =
        budgetService.CalculatePercentageUsed(
            budget,
            transactions);

    string status =
        budgetService.GetBudgetStatus(
            budget,
            transactions);

    Console.WriteLine();
    Console.WriteLine(
        $"Monthly Budget:   {budget:N2} {budgetCurrency}");

    Console.WriteLine(
        $"Total Expenses:   {expenses:N2} {budgetCurrency}");

    Console.WriteLine(
        $"Remaining:        {remaining:N2} {budgetCurrency}");

    Console.WriteLine(
        $"Budget Used:      {percentageUsed:F2}%");

    Console.WriteLine(
        $"Status:           {status}");

    Pause();
}
string FormatTransactionAmount(
    decimal amount,
    CurrencyCode currency)
{
    string symbol = currency switch
    {
        CurrencyCode.NGN => "₦",
        CurrencyCode.USD => "$",
        CurrencyCode.GBP => "£",
        CurrencyCode.EUR => "€",
        CurrencyCode.CAD => "CA$",
        CurrencyCode.AUD => "A$",
        CurrencyCode.CHF => "CHF ",
        CurrencyCode.JPY => "¥",
        CurrencyCode.CNY => "CN¥",
        CurrencyCode.INR => "₹",
        CurrencyCode.ZAR => "R",
        CurrencyCode.AED => "AED ",
        CurrencyCode.SAR => "SAR ",
        CurrencyCode.KES => "KSh ",
        CurrencyCode.GHS => "GH₵",
        CurrencyCode.EGP => "E£",
        CurrencyCode.BRL => "R$",
        CurrencyCode.MXN => "MX$",
        CurrencyCode.SGD => "S$",
        CurrencyCode.NZD => "NZ$",
        CurrencyCode.SEK => "SEK ",
        CurrencyCode.NOK => "NOK ",
        CurrencyCode.DKK => "DKK ",
        CurrencyCode.PLN => "zł ",
        CurrencyCode.TRY => "₺",
        _ => $"{currency} "
    };

    return $"{symbol}{amount:N2}";
}