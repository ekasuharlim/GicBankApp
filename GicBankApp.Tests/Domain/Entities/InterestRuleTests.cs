namespace GicBankApp.Tests.Domain.Entities;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;

public class InterestRuleTests
{
    [Fact]
    public void InterestRulePercentage_ShouldCreateInterestRuleIfValidAmount()
    {
        var result = InterestRule.Create(
            BusinessDate.From("20230626"), 
            "RULE01", 
            5.0m);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(BusinessDate.From("20230626").Value, result.Value!.EffectiveDate.Value);
        Assert.Equal("RULE01", result.Value!.RuleId);
        Assert.Equal(5.0M, result.Value!.RatePercentage);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(100.0)]
    [InlineData(-1.0)]
    [InlineData(101.0)]
    public void InterestRulePercentage_ShouldReturnErrorIfNotValidAmount(decimal amount)
    {
        var result = InterestRule.Create(
            BusinessDate.From("20230626"), 
            "RULE01", 
            amount);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.NotNull(result.Error);
        Assert.Equal(Error.InvalidInterestRateAmount.Code, result.Error!.Code);
    }


}