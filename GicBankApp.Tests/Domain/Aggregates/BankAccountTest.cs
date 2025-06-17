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

        var transaction = new WithdrawalTransaction(
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

    [Fact]
    public void Withdrawal_Transaction_ShouldSucceed_If_Sufficient_Balance()
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
            new Money(99.00m));
        Result<Transaction> withdrawalAction = account.AddTransaction(withDrawal);
        Assert.True(withdrawalAction.IsSuccess);
        Assert.NotNull(withdrawalAction.Value);
        Assert.Equal(withdrawalAction.Value!.Amount, withDrawal.Amount);
        Assert.Equal(1.00m, account.LatestBalance.Value);
    }



    [Fact]
    public void GetBalanceBeforeDate_ShouldReturnZero_IfNoTransactions()
    {
        var account = new BankAccount("AC001");
        var date = BusinessDate.From("20230626");

        var balance = account.GetBalanceBeforeDate(date);

        Assert.Equal(0, balance.Value);
    }
    [Fact]
    public void GetBalanceBeforeDate_ShouldReturnCorrectBalance()
    {
        var account = new BankAccount("AC001");
        var date1 = BusinessDate.From("20230626");
        var date2 = BusinessDate.From("20230627");

        var deposit1 = new DepositTransaction(
            date1,
            new TransactionId(date1, 1),
            new Money(100.00m));
        account.AddTransaction(deposit1);

        var deposit2 = new DepositTransaction(
            date2,
            new TransactionId(date2, 2),
            new Money(50.00m));
        account.AddTransaction(deposit2);

        var balance = account.GetBalanceBeforeDate(date2);

        Assert.Equal(100.00m, balance.Value);
    }
    [Fact]
    public void GetBalanceBeforeDate_ShouldReturnZero_IfNoTransactionsBeforeDate()
    {
        var account = new BankAccount("AC001");
        var date1 = BusinessDate.From("20230626");
        var date2 = BusinessDate.From("20230425");

        var deposit1 = new DepositTransaction(
            date1,
            new TransactionId(date1, 1),
            new Money(100.00m));
        account.AddTransaction(deposit1);

        var balance = account.GetBalanceBeforeDate(date2);

        Assert.Equal(0, balance.Value);
    }
    [Fact]
    public void GetBalanceBeforeDate_ShouldReturnCorrectBalance_IfMultipleTransactions()
    {
        var account = new BankAccount("AC001");
        var date1 = BusinessDate.From("20230626");
        var date2 = BusinessDate.From("20230627");
        var date3 = BusinessDate.From("20230628");

        var deposit1 = new DepositTransaction(
            date1,
            new TransactionId(date1, 1),
            new Money(100.00m));
        account.AddTransaction(deposit1);

        var deposit2 = new DepositTransaction(
            date2,
            new TransactionId(date2, 2),
            new Money(50.00m));
        account.AddTransaction(deposit2);

        var withdrawal = new WithdrawalTransaction(
            date3,
            new TransactionId(date3, 3),
            new Money(30.00m));
        account.AddTransaction(withdrawal);

        var balance = account.GetBalanceBeforeDate(date3);

        Assert.Equal(150.00m, balance.Value);
    }

    [Fact]
    public void BankAccount_AddDepositTransactionShouldSuccessWhenDateIsEarlierThanFirstTransaction()
    {
        var account = new BankAccount("AC001");
        var date1 = BusinessDate.From("20230626");
        var date2 = BusinessDate.From("20200627");
        var date3 = BusinessDate.From("20230628");

        var deposit1 = new DepositTransaction(
            date1,
            new TransactionId(date1, 1),
            new Money(100.00m));
        account.AddTransaction(deposit1);

        var deposit2 = new DepositTransaction(
            date2,
            new TransactionId(date2, 2),
            new Money(50.00m));
        account.AddTransaction(deposit2);

        var balance = account.GetBalanceBeforeDate(date3);
        Assert.Equal(150.00m, balance.Value);
    }

    [Fact]
    public void BankAccount_AddWithdrawalTransactionShouldSuccess()
    {
        var account = new BankAccount("AC001");
        var date1 = BusinessDate.From("20230626");
        var date2 = BusinessDate.From("20200627");
        var date3 = BusinessDate.From("20230628");

        var deposit1 = new DepositTransaction(
            date1,
            new TransactionId(date1, 1),
            new Money(100.00m));
        account.AddTransaction(deposit1);

        var deposit2 = new WithdrawalTransaction(
            date2,
            new TransactionId(date2, 2),
            new Money(30.00m));

        Result<Transaction> withdrawalAction = account.AddTransaction(deposit2);
        Assert.True(withdrawalAction.IsSuccess);
        var balance = account.GetBalanceBeforeDate(date3);
        Assert.Equal(70.00m, balance.Value);        
    }

}
