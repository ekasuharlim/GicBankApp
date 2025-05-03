namespace GicBankApp.ConsoleUi.Printers;

using GicBankApp.Application.Dtos;

public class InterestRuleListPrinter : IPrinter
{
    private readonly IEnumerable<InterestRuleDto> _interestRules;
    public InterestRuleListPrinter(IEnumerable<InterestRuleDto> interestRules)
    {
        _interestRules = interestRules;
    }
    public void Print()
    {
        if(_interestRules == null || !_interestRules.Any())
        {
            Console.WriteLine("No interest rules found.");
            return;
        }

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("Interest rules:");
        Console.WriteLine("| Date     | RuleId | Rate (%) |");

        foreach (var rule in _interestRules)
        {
            Console.WriteLine($"| {rule.EffectiveDate} | {rule.RuleId} | {rule.RatePercent,8:F2} |");
        }
        Console.WriteLine();
    }
}
