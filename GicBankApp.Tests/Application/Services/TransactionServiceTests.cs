
namespace GicBankApp.Tests.Application.Services;

using GicBankApp.Application.Services;
using GicBankApp.Domain.Factories;
using GicBankApp.Domain.Common;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;

public class TransactionServiceTests
{
    private readonly Mock<IBankAccountRepository> _accountRepoMock;
    private readonly Mock<ITransactionFactory> _transactionFactoryMock;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _accountRepoMock = new Mock<IBankAccountRepository>();
        _transactionFactoryMock = new Mock<ITransactionFactory>();

        _service = new TransactionService(
            _accountRepoMock.Object,
            _transactionFactoryMock.Object);
    }

    [Fact]
    public async Task AddTransactionAsync_ShouldCreateNewAccountAndAddTransaction_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = "AC001";
        var amount = 100m;
        var date = "20230626";
        var type = "D";
        var money = new Money(amount);

        
        var transaction = new DepositTransaction(
            BusinessDate.From(date), 
            new TransactionId(BusinessDate.From(date),1), 
            money);

        _accountRepoMock.Setup(r => r.GetByIdAsync(accountId))
                        .ReturnsAsync((BankAccount?)null);

        _transactionFactoryMock.Setup(f => f.CreateTransaction(
                It.IsAny<BusinessDate>(), 
                It.IsAny<Money>(), 
                type))
            .Returns(transaction);

        BankAccount savedAccount = null!;
        _accountRepoMock.Setup(r => r.SaveAsync(It.IsAny<BankAccount>()))
                        .Callback<BankAccount>(a => savedAccount = a)
                        .Returns(Task.CompletedTask);

        var result = await _service.AddTransactionAsync(date, accountId, type, amount);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(accountId, result.Value.AccountId);
        Assert.Single(result.Value.Transactions);
        Assert.Single(result.Value.Transactions);
        Assert.Equal(amount, result.Value.Transactions[0].Amount);
        Assert.Equal("D", result.Value.Transactions[0].Type);

    }

    [Fact]
    public async Task AddTransactionAsync_ShouldAddTransactionToExistingAccount()
    {
        // Arrange
        var accountId = "AC002";
        var date = "20230627";
        var type = "D";
        var amount = 50m;
        var money = new Money(amount);
        var existingAccount = new BankAccount(accountId);
        var transaction = new DepositTransaction(
            BusinessDate.From(date), 
            new TransactionId(BusinessDate.From(date),1), 
            money);

        _accountRepoMock.Setup(r => r.GetByIdAsync(accountId))
                        .ReturnsAsync(existingAccount);

        _transactionFactoryMock.Setup(f => f.CreateTransaction(
                It.IsAny<BusinessDate>(), 
                It.IsAny<Money>(), 
                type))
            .Returns(transaction);

        var result = await _service.AddTransactionAsync(date, accountId, type, amount);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(accountId, result.Value.AccountId);
        Assert.Single(result.Value.Transactions);
        Assert.Equal(amount, result.Value.Transactions[0].Amount);
        Assert.Equal("D", result.Value.Transactions[0].Type);
    }

            [Fact]
    public async Task AddTransactionAsync_ShouldReturnErrorIfTransactionNotValid()
    {
        var accountId = "AC002";
        var date = "20230627";
        var type = "W";
        var amount = 50m;
        var money = new Money(amount);
        var existingAccount = new BankAccount(accountId);
        var transaction = new WithdrawalTransaction(
            BusinessDate.From(date), 
            new TransactionId(BusinessDate.From(date),1), 
            money);

        _accountRepoMock.Setup(r => r.GetByIdAsync(accountId))
                        .ReturnsAsync(existingAccount);

        _transactionFactoryMock.Setup(f => f.CreateTransaction(
                It.IsAny<BusinessDate>(), 
                It.IsAny<Money>(), 
                type))
            .Returns(transaction);

        var result = await _service.AddTransactionAsync(date, accountId, type, amount);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }
}