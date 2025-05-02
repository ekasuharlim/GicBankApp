namespace GicBankApp.ConsoleUi.Menu;

using GicBankApp.Application.Services;
using System.Globalization;
using GicBankApp.Domain.Factories;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.Common;
using GicBankApp.ConsoleUi.Printers;

public class TransactionInputMenu
{
    private readonly TransactionService _transactionService;   
    public TransactionInputMenu(
        IBankAccountRepository accountRepo, 
        ITransactionFactory transactionFactory)
    {
        _transactionService = 
            new TransactionService(accountRepo, transactionFactory);
    }

    public async void Start()
    {
        Console.Clear();
        Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format");
        Console.WriteLine("(or enter blank to go back to main menu):");

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return;

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4)
            {
                Console.WriteLine("Invalid format. Try again.");
                continue;
            }

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, DateTimeStyles.None, out var date) ||
                !decimal.TryParse(parts[3], out var amount) ||
                amount <= 0)
            {
                Console.WriteLine("Invalid input. Try again.");
                continue;
            }

            var dateStr = parts[0];
            var accountId = parts[1];
            var type = parts[2].ToUpper();

            try
            {
                var result = await _transactionService.AddTransactionAsync(
                    dateStr, 
                    accountId, 
                    type, 
                    amount);

                if (result.IsSuccess) {
                    var account = result.Value;
                    AccountStatementPrinter.Print(account);
                    return;
                } 
                else
                {
                    Console.WriteLine($"Error: {result.Error.Message}");
                    return;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
