namespace GicBankApp.Tests.Application.Mappers;

using GicBankApp.Application.Dtos;
using GicBankApp.Application.Mappers;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
public class InterestRuleMapperTests
{
    [Fact]
    public void ToDto_Should_Map_DomainEntity_To_Dto_Correctly()
    {
        var date = BusinessDate.From("20230615");
        var rule = InterestRule.Create(date, "RULE01", 2.20m);

        Assert.True(rule.IsSuccess);
        Assert.NotNull(rule.Value);
        var dto = InterestRuleMapper.ToDto(rule.Value);
        Assert.Equal("20230615", dto.EffectiveDate);
        Assert.Equal("RULE01", dto.RuleId);
        Assert.Equal(2.20m, dto.RatePercent);
    }

}