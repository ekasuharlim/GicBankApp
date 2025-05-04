# GicBankApp

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Getting Started

### Installation

1. Clone the repository
2. Navigate to the project directory

### Commands

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run tests
dotnet test

# Run the application
dotnet run --project ./GicBankApp/GicBankApp.csproj
```

Assumptions:
- User can't enter back dated transaction
- If no interest rate can be applied , it will have 0 interest rate

