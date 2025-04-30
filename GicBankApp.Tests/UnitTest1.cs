namespace GicBankApp.Tests;

using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;

public class BankAccountTest
{
     [Fact]
    public void FirstTransaction_ShouldFail_IfWithdrawal()
    {
        var account = new BankAccount("AC001");

        var transaction = new Transaction(
            transactionId: "TX001",
            date: new DateTime(2023, 06, 26),
            type: TransactionType.Withdrawal,
            amount: 100.00m
        );

        var result = account.AddTransaction(transaction);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(Error.FirstTransactionCannotBeWithdrawal.Code, result.Error!.Code);
    }


}
