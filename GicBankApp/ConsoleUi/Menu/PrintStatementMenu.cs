namespace GicBankApp.ConsoleUi.Menu;

using GicBankApp.Application.Interfaces;
using GicBankApp.ConsoleUi.Printers;
using GicBankApp.Shared;

public class PrintStatementMenu : IMenu
{

    public PrintStatementMenu(IPrintStatementService printStatementService)
    {
        _printStatementService = printStatementService;
    }

    private readonly IPrintStatementService _printStatementService;

    public async void Start()
    {
        while (true)
        {
            Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>");
            Console.WriteLine("(or enter blank to go back to main menu):");
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            var parts = input.Trim().Split(' ');
            if (parts.Length != 2 || parts[1].Length != 6 || !int.TryParse(parts[1], out var ym))
            {
                Console.WriteLine("Invalid input format. Please try again.");
                continue;
            }

            var accountId = parts[0];
            var year = ym / 100;
            var month = ym % 100;

            var result = await _printStatementService.PrintStatementAsync(accountId, year, month);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"Error: {result.Error.Message}");
                continue;
            }

            var statement = result.Value;
        
            var printer = new AccountStatementPrinter(statement);
            printer.Print();
            return;

        }
    }
}