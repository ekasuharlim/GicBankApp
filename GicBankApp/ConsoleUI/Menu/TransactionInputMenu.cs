using System.Globalization;

namespace GicBankApp.ConsoleUI.Menu;
public class TransactionInputMenu
{
    public void Start()
    {
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

            var accountId = parts[1];
            var type = parts[2].ToUpper();

            try
            {
                // Call Application Service
                //var transaction = ApplicationServices.Transactions.AddTransaction(date, accountId, type, amount);

                // Display statement
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
