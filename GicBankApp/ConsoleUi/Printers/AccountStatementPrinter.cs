
namespace GicBankApp.ConsoleUi.Printers;

using GicBankApp.Application.Dtos;

public class AccountStatementPrinter : IPrinter
{
    private readonly AccountStatementDto _accountStatement;

    public AccountStatementPrinter(AccountStatementDto accountStatement) {
        _accountStatement = accountStatement;
    }
    public void Print()
    {
        Console.WriteLine($"Account: {_accountStatement.AccountId}");
        Console.WriteLine("| Date     | Txn Id        | Type | Amount   | Balance   |");

        foreach (var txn in _accountStatement.Transactions)
        {
            Console.WriteLine(
                $"| {txn.Date,-9} | {txn.TransactionId,-12} | {txn.Type,-4} | {txn.Amount,8:0.00} | {txn.Balance,9:0.00} |");
        }
    }
}