namespace GicBankApp.Tests.Domain.Aggregates;

using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;
using GicBankApp.Domain.Common;
using GicBankApp.Domain.Factories;

public class BankAccountTest
{
     [Fact]
    public void FirstTransaction_ShouldFail_IfWithdrawal()
    {
        var account = new BankAccount("AC001");
        var date = BusinessDate.From("20230626");         

        var transaction = new DepositTransaction(
            date, 
            new TransactionId(date, 1), 
            new Money(100.00m)); 

        var result = account.AddTransaction(transaction);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(Error.FirstTransactionCannotBeWithdrawal.Code, result.Error!.Code);
    }

    [Fact]
    public void FirstTransaction_ShouldSucceed_IfDeposit()
    {
        var account = new BankAccount("AC001");

        var date = BusinessDate.From("20230626"); 

        var transaction = new DepositTransaction(
            date, 
            new TransactionId(date, 1), 
            new Money(100.00m)); 


        Result<Transaction> result = account.AddTransaction(transaction);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(result.Value!.Amount, transaction.Amount);
    }

    [Fact]
    public void Withdrawal_Transaction_ShouldFail_If_Insufficent_Balance()
    {
        var account = new BankAccount("AC001");
        var date = BusinessDate.From("20230626"); 

        var deposit = new DepositTransaction(
            date, 
            new TransactionId(date, 1), 
            new Money(100.00m)); 


        Result<Transaction> depositAction = account.AddTransaction(deposit);
        Assert.True(depositAction.IsSuccess);

        var withDrawal = new WithdrawalTransaction(
            date, 
            new TransactionId(date, 1), 
            new Money(101.00m)); 


        Result<Transaction> withdrawalAction = account.AddTransaction(withDrawal);
        Assert.False(withdrawalAction.IsSuccess);
        Assert.NotNull(withdrawalAction.Error);
        Assert.Equal(Error.InsufficentBalance.Code, withdrawalAction.Error!.Code);

    }


}
