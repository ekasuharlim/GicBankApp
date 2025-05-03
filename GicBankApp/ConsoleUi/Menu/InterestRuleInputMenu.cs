namespace GicBankApp.ConsoleUi.Menu;

using System.Globalization;
using GicBankApp.Application.Interfaces;
using GicBankApp.Domain.Entities;
using GicBankApp.ConsoleUi.Printers;

public class InterestRuleInputMenu 
{
    private readonly IInterestRuleService _interestRuleService;

    public InterestRuleInputMenu(IInterestRuleService interestRuleService)
    {
        _interestRuleService = interestRuleService;
    }

    public async void Start()
    {
        while (true)
        {
            Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format ");
            Console.WriteLine("(or enter blank to go back to main menu):");
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return;

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                Console.WriteLine("Invalid input format. Try again.");
                continue;
            }

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, DateTimeStyles.None, out var date) ||
                !decimal.TryParse(parts[2], out var amount) ||
                amount <= 0)
            {
                Console.WriteLine("Invalid input. Try again.");
                continue;
            }


            var result = await _interestRuleService.AddInterestRuleAsync(parts[0], parts[1],amount);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"Error: {result.Error.Message}");
                continue;
            }


            var allRules = await _interestRuleService.GetAllInterestRulesAsync();

            if (!allRules.IsSuccess)
            {
                Console.WriteLine($"Error: {allRules.Error}");
                return;
            }

            var rules = allRules.Value;
            var printer = new InterestRuleListPrinter(rules);
            printer.Print();

            return;
        }
    }
}