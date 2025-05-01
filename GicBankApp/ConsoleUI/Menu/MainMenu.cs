namespace GicBankApp.ConsoleUI.Menu;

public class MainMenu : IMenu
{
    public void Start()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            Console.WriteLine("[T] Input transactions");
            Console.WriteLine("[I] Define interest rules");
            Console.WriteLine("[P] Print statement");
            Console.WriteLine("[Q] Quit");
            Console.Write("> ");

            var choice = Console.ReadLine()?.Trim().ToUpperInvariant();

            switch (choice)
            {
                case "T":
                    new TransactionInputMenu().Start();
                    break;
                case "I":
                    throw new NotImplementedException("Interest rule definition is not implemented yet.");
                case "P":
                    throw new NotImplementedException("Print statement is not implemented yet.");
                case "Q":
                    Console.WriteLine("Thank you for banking with AwesomeGIC Bank.");
                    Console.WriteLine("Have a nice day!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
