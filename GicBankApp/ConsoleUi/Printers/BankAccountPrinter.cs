namespace GicBankApp.ConsoleUi.Printers;

using GicBankApp.Application.Dtos;

public class AccountStatementPrinter : IPrinter
{
    private readonly BankAccountDto _bankAccount;

    public AccountStatementPrinter(BankAccountDto account)
    {
        _bankAccount = account;
    }
    public void Print()
    {
        if (_bankAccount == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }
        
        Console.Clear();
        Console.WriteLine($"Account: {_bankAccount.AccountId}");
        Console.WriteLine("| Date     | Txn Id      | Type | Amount |");
        foreach (var t in _bankAccount.Transactions)
        {
            Console.WriteLine($"| {t.Date} | {t.TransactionId,-11} | {t.Type,-4} | {t.Amount,6:0.00} |");
        }
    }

}