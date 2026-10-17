# FinanceTracker

A C#/.NET personal finance tracking application built to demonstrate software engineering fundamentals, transaction management, monthly financial summaries, budget monitoring, and persistent data storage.

## Features

- **Transaction management:** Record and manage financial transactions.
- **Monthly summaries:** Review monthly financial activity with currency formatting.
- **Budget tracking:** Monitor budgets and receive budget status alerts.
- **Persistent storage:** Save transactions in JSON so they remain available between application runs.
- **Automated tests:** Use the test project to verify application behavior.
- **Layered architecture:** Organize application logic, infrastructure, and the console interface into separate projects.

## Technology Stack

- C#
- .NET
- JSON
- Git and GitHub
- Visual Studio / Visual Studio Code

## Project Structure

The solution contains the following projects:

- `FinanceTracker.Console` — Console application and user interaction.
- `FinanceTracker.Core` — Core application logic and services.
- `FinanceTracker.Infrastructure` — Data persistence and infrastructure components.
- `FinanceTracker.Tests` — Automated tests.

The solution file is `FinanceTracker.sln`.

## Data Persistence

FinanceTracker uses JSON-based transaction storage. Transaction data is saved locally under the current Windows user's Local Application Data directory:

`%LOCALAPPDATA%\FinanceTracker\transactions.json`

This allows saved transactions to persist when the application is closed and reopened.

## Getting Started

### Prerequisites

- A compatible .NET SDK.
- Windows, PowerShell, and a terminal.
- Git, if cloning the repository.

### Run the application

1. Clone the repository:

   ```bash
   git clone https://github.com/ewohbenjaminf/FinanceTracker.git
   ```

2. Open the project directory:

   ```bash
   cd FinanceTracker
   ```

3. Restore dependencies:

   ```bash
   dotnet restore FinanceTracker.sln
   ```

4. Build the solution:

   ```bash
   dotnet build FinanceTracker.sln
   ```

5. Run the console application:

   ```bash
   dotnet run --project src/FinanceTracker.Console/FinanceTracker.Console.csproj
   ```

The command above uses the console project path in this repository.

## Running Tests

Run the solution's tests with:

```bash
dotnet test FinanceTracker.sln
```

## Future Improvements

Potential improvements include:

- Expense categorization and filtering.
- Exportable financial reports.
- Enhanced validation and error handling.
- Additional automated tests.
- Secure backup and restore options.
- A graphical user interface.

## Author

**Ewoh Benjamin Friday**

GitHub: [@ewohbenjaminf](https://github.com/ewohbenjaminf)

## License

See the [LICENSE](LICENSE) file for license information.

