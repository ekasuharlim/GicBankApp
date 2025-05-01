namespace GicBankApp.ConsoleUi.Printers;

using GicBankApp.Application.Dtos;

public static class AccountStatementPrinter
{
    public static void Print(BankAccountDto? account)
    {
        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }
        Console.Clear();
        Console.WriteLine($"Account: {account.AccountId}");
        Console.WriteLine("| Date     | Txn Id      | Type | Amount |");
        foreach (var t in account.Transactions)
        {
            Console.WriteLine($"| {t.Date} | {t.TransactionId,-11} | {t.Type,-4} | {t.Amount,6:0.00} |");
        }
    }

}