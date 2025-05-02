
namespace GicBankApp.ConsoleUi.Menu;

using GicBankApp.Application.Interfaces;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.Common;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.Factories;
using GicBankApp.Infrastructure.Repository;
using GicBankApp.Infrastructure.Services;
using GicBankApp.Application.Services;

public class MainMenu : IMenu
{
    IBankAccountRepository _accountRepo;
    ITransactionIdGenerator _idGenerator;
    ITransactionFactory _transactionFactory; 

    IInterestRuleRepository _interestRuleRepo;
    ITransactionService _transactionService;
    IInterestRuleService _interestRuleService;

    public MainMenu()
    {
        _accountRepo = new BankAccountRepository();
        _idGenerator = new TransactionIdGenerator();
        _transactionFactory = new TransactionFactory(_idGenerator);
        _transactionService = new TransactionService(_accountRepo, _transactionFactory);
        _interestRuleRepo = new InterestRuleRepository();
        _interestRuleService = new InterestRuleService(_interestRuleRepo);
        
    }

    public void Start()
    {
        while (true)
        {
            Console.WriteLine("[T] Input transactions");
            Console.WriteLine("[I] Define interest rules");
            Console.WriteLine("[P] Print statement");
            Console.WriteLine("[Q] Quit");
            Console.Write("> ");

            var choice = Console.ReadLine()?.Trim().ToUpperInvariant();

            switch (choice)
            {
                case "T":
                    new TransactionInputMenu(_transactionService).Start();
                    break;
                case "I":
                    new InterestRuleInputMenu(_interestRuleService).Start();
                    break;
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
            
            Console.WriteLine();
            Console.WriteLine("Is there anything else you would like to do?");
        }
    }
}
