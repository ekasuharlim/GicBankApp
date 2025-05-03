
namespace GicBankApp.Tests.Application.Mappers;

using GicBankApp.Application.Mappers;
using GicBankApp.Application.Dtos;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;


 public class TransactionMapperTests
{
    [Fact]
    public void ToDto_Should_Map_DepositTransaction_Correctly()
    {
        var date = BusinessDate.From("20230626");
        var txnId = new TransactionId(date, 1);
        var amount = new Money(100.00m);
        var deposit = new DepositTransaction(date, txnId, amount);

        var dto = TransactionMapper.ToDto(deposit);

        Assert.Equal("20230626", dto.Date);
        Assert.Equal("20230626-01", dto.TransactionId);
        Assert.Equal("D", dto.Type);
        Assert.Equal(100.00m, dto.Amount);
    }

    [Fact]
    public void ToDto_Should_Map_WithdrawalTransaction_Correctly()
    {
        var date = BusinessDate.From("20230626");
        var txnId = new TransactionId(date, 2);
        var amount = new Money(50.00m);
        var withdrawal = new WithdrawalTransaction(date, txnId, amount);

        var dto = TransactionMapper.ToDto(withdrawal);

        Assert.Equal("20230626", dto.Date);
        Assert.Equal("20230626-02", dto.TransactionId);
        Assert.Equal("W", dto.Type);
        Assert.Equal(50.00m, dto.Amount);
    }
}