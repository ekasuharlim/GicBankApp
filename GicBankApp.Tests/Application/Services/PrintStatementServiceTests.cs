namespace GicBankApp.Tests.Application.Services;

using GicBankApp.Application.Dtos;
using GicBankApp.Application.Interfaces;    
using GicBankApp.Application.Services;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;    
using GicBankApp.Domain.Aggregates;
using GicBankApp.Infrastructure.Services;
using GicBankApp.Domain.Factories;

public class PrintStatementServiceTests
{
    private readonly Mock<IBankAccountRepository> _accountRepoMock;
    private readonly Mock<IInterestCalculatorService> _interestCalculatorMock;
    private readonly PrintStatementService _service;    
    private readonly TransactionFactory _transactionFactory;    

   public PrintStatementServiceTests()
    {
        _accountRepoMock = new Mock<IBankAccountRepository>();
        _interestCalculatorMock = new Mock<IInterestCalculatorService>();
        _service = new PrintStatementService(_accountRepoMock.Object, _interestCalculatorMock.Object);
        _transactionFactory = new TransactionFactory(new TransactionIdGenerator());

    }    

    
  [Fact]
    public async Task PrintStatementAsync_ReturnsFailure_WhenAccountNotFound()
    {
        _accountRepoMock.Setup(r => r.GetByIdAsync("AC001"))
                        .ReturnsAsync((BankAccount)null);

        var result = await _service.PrintStatementAsync("AC001", 2023, 6);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.BankAccountNotFound, result.Error);
    }

    [Fact]
    public async Task PrintStatementAsync_ReturnsFailure_WhenInvalidMonth()
    {
        var result = await _service.PrintStatementAsync("AC001", 2023, 13); // Invalid month

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task PrintStatementAsync_ReturnsSuccess_WithCorrectTransactionData()
    {
        var account = new BankAccount("AC001");

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230601"), 
            new Money(150.00m), 
            "D"));

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230610"), 
            new Money(50.00m), 
            "W"));


        _accountRepoMock.Setup(r => r.GetByIdAsync("AC001"))
                        .ReturnsAsync(account);

        _interestCalculatorMock.Setup(c => c.CalculateTotalInterestAsync(account, It.IsAny<MonthlyPeriod>()))
                               .ReturnsAsync(0.39m);

        var result = await _service.PrintStatementAsync("AC001", 2023, 6);

        Assert.True(result.IsSuccess);
        var dto = result.Value;
        Assert.Equal("AC001", dto.AccountId);
        Assert.Equal(3, dto.Transactions.Count); 

        Assert.Equal("D", dto.Transactions[0].Type);
        Assert.Equal(150m, dto.Transactions[0].Amount);

        Assert.Equal("W", dto.Transactions[1].Type);
        Assert.Equal(50m, dto.Transactions[1].Amount);

        Assert.Equal("I", dto.Transactions[2].Type);
        Assert.Equal(0.39m, dto.Transactions[2].Amount);
        Assert.Equal(string.Empty, dto.Transactions[2].TransactionId);
    }    
}