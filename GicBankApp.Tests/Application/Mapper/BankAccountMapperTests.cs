namespace GicBankApp.Tests.Application.Mappers;

using GicBankApp.Application.Mappers;
using GicBankApp.Application.Dtos;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;
public class BankAccountMapperTests
{
    [Fact]
    public void ToDto_Should_Map_BankAccount_With_Transactions_Correctly()
    {
        // Arrange
        var accountId = "AC001";
        var depositDate = BusinessDate.From("20230601");
        var withdrawDate =BusinessDate.From("20230602");
        var deposit = new DepositTransaction(depositDate, new TransactionId(depositDate,1), 
            new Money(200m));
        var withdrawal = new WithdrawalTransaction(withdrawDate, new TransactionId(withdrawDate,1),
            new Money(50m));

        var account = new BankAccount(accountId);
        account.AddTransaction(deposit);
        account.AddTransaction(withdrawal);

        var dto = BankAccountMapper.ToDto(account);

        Assert.Equal(accountId, dto.AccountId);
        Assert.Equal(150m, dto.LatestBalance);

        Assert.Collection(dto.Transactions,
            txn =>
            {
                Assert.Equal("20230601", txn.Date);
                Assert.Equal("20230601-01", txn.TransactionId);
                Assert.Equal("D", txn.Type);
                Assert.Equal(200m, txn.Amount);
            },
            txn =>
            {
                Assert.Equal("20230602", txn.Date);
                Assert.Equal("20230602-01", txn.TransactionId);
                Assert.Equal("W", txn.Type);
                Assert.Equal(50m, txn.Amount);
            });
    }

}