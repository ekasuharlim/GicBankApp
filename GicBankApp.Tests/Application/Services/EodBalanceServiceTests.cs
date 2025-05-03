namespace GicBankApp.Tests.Application.Services;

using GicBankApp.Application.Interfaces;
using GicBankApp.Application.Services;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Domain.Entities;
using GicBankApp.Infrastructure.Services;
using GicBankApp.Domain.Factories;

public class EodBalanceServiceTests
{
    private readonly EodBalanceService _eodBalanceService;
    private readonly TransactionFactory _transactionFactory;

    public EodBalanceServiceTests()
    {
        _eodBalanceService = new EodBalanceService();
        _transactionFactory = new TransactionFactory(new TransactionIdGenerator());
    }

    [Fact] 
    public void CalculateEodBalances_ShouldReturnCorrectBalances()
    {
        var account = new BankAccount("AC001");

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230505"), 
            new Money(100.00m), 
            "D"));

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230601"), 
            new Money(150.00m), 
            "D"));

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230626"), 
            new Money(20.00m), 
            "W"));
        
        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230626"), 
            new Money(100.00m), 
            "W"));

        DateTime startDate = new DateTime(2023, 6, 1);
        DateTime endDate = new DateTime(2023, 6, 30);   

        var result = _eodBalanceService.CalculateEodBalances(account, startDate, endDate);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.True(result.ContainsKey(new DateTime(2023, 6, 1)));
        Assert.True(result.ContainsKey(new DateTime(2023, 6, 26)));
        Assert.Equal(250.00m, result[new DateTime(2023, 6, 1)]);
        Assert.Equal(130.00m, result[new DateTime(2023, 6, 26)]);
        
    }

    [Fact]
    public void CalculateEodBalances_ShouldReturnEmpty_WhenNoTransactionsInDateRange()
    {
        var account = new BankAccount("AC001");

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230505"), 
            new Money(100.00m), 
            "D"));

        DateTime startDate = new DateTime(2023, 6, 1);
        DateTime endDate = new DateTime(2023, 6, 30);   

        var result = _eodBalanceService.CalculateEodBalances(account, startDate, endDate);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
    [Fact]
    public void CalculateEodBalances_ShouldReturnEmpty_WhenAccountHasNoTransactions()
    {
        var account = new BankAccount("AC001");

        DateTime startDate = new DateTime(2023, 6, 1);
        DateTime endDate = new DateTime(2023, 6, 30);   

        var result = _eodBalanceService.CalculateEodBalances(account, startDate, endDate);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
    [Fact]
    public void CalculateEodBalances_ShouldReturnEmpty_WhenDateRangeIsInvalid()
    {
        var account = new BankAccount("AC001");

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230505"), 
            new Money(100.00m), 
            "D"));

        DateTime startDate = new DateTime(2023, 6, 30);
        DateTime endDate = new DateTime(2023, 6, 1);   

        var result = _eodBalanceService.CalculateEodBalances(account, startDate, endDate);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
    [Fact]
    public void CalculateEodBalances_ShouldReturnEmpty_WhenDateRangeIsSameDay()
    {
        var account = new BankAccount("AC001");

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230601"), 
            new Money(100.00m), 
            "D"));

        DateTime startDate = new DateTime(2023, 6, 1);
        DateTime endDate = new DateTime(2023, 6, 1);   

        var result = _eodBalanceService.CalculateEodBalances(account, startDate, endDate);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.True(result.ContainsKey(new DateTime(2023, 6, 1)));
        Assert.Equal(100.00m, result[new DateTime(2023, 6, 1)]);
    }

}