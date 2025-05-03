namespace GicBankApp.ConsoleUi.Menu;

using GicBankApp.Application.Services;
using System.Globalization;
using GicBankApp.ConsoleUi.Printers;
using GicBankApp.Application.Interfaces;

public class TransactionInputMenu
{
    private readonly ITransactionService _transactionService;   
    public TransactionInputMenu(
        ITransactionService transactionService)
    {
        _transactionService = transactionService;
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
                    var printer = new BankAccountPrinter(account);
                    printer.Print();
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
