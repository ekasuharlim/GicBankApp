namespace GicBankApp.Tests.Domain.Entities;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;

public class TransactionTests
{
    [Fact]
    public void DepositTransaction_ShouldIncreaseBalance()
    {

        var date = BusinessDate.From("20230626"); 
        var initialBalance = new Money(50.00m);

        var transaction = new DepositTransaction(
            date, 
            new TransactionId(date, 1), 
            new Money(100.00m)); 

        var newBalance = transaction.GetBalance(initialBalance);

        Assert.Equal(new Money(150.00m).Value, newBalance.Value);
    }

    [Fact]
    public void WithdrawalTransaction_ShouldDecreaseBalance()
    {
        var date = BusinessDate.From("20230626"); 
        var initialBalance = new Money(150.00m);

        var transaction = new WithdrawalTransaction(
            date, 
            new TransactionId(date, 1), 
            new Money(100.00m)); 

        var newBalance = transaction.GetBalance(initialBalance);

        Assert.Equal(new Money(50.00m).Value, newBalance.Value);
    }


}