namespace GicBankApp.Tests.Application.Services;

using System.ComponentModel;
using System.IO.Pipes;
using GicBankApp.Application.Services;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;

public class InterestRuleServiceTests
{
    private readonly Mock<IInterestRuleRepository> _repoMock;
    private readonly InterestRuleService _service;

    public InterestRuleServiceTests()
    {
        _repoMock = new Mock<IInterestRuleRepository>();
        _service = new InterestRuleService(_repoMock.Object);
    }

    [Fact]
    public async Task AddInterestRuleAsync_Should_Add_New_Rule_When_Valid()
    {
        var dateStr = "20230615";
        var ruleId = "RULE01";
        var rate = 2.20m;
        var date = BusinessDate.From(dateStr);
        var expectedRule = InterestRule.Create(date, ruleId, rate).Value;

        _repoMock.Setup(r => r.GetInterestRuleByDateAsync(date))
                 .ReturnsAsync(new List<InterestRule>());

        var result = await _service.AddInterestRuleAsync(dateStr, ruleId, rate);

        Assert.True(result.IsSuccess);
        Assert.Equal(ruleId, result.Value.RuleId);
        Assert.Equal(dateStr, result.Value.EffectiveDate);
        Assert.Equal(rate, result.Value.RatePercent);

        _repoMock.Verify(r => r.SaveAsync(It.IsAny<InterestRule>()), Times.Once);
    }

    [Fact]
    public async Task AddInterestRuleAsync_Should_Remove_Existing_Rules_Before_Adding()
    {
        var dateStr = "20230615";
        var ruleId = "RULE02";
        var rate = 1.50m;
        var date = BusinessDate.From(dateStr);
        var existing = InterestRule.Create(date, "OLD", 1.0m);

        Assert.NotNull(existing.Value);

        _repoMock.Setup(r => r.GetInterestRuleByDateAsync(It.Is<BusinessDate>(d => d.Value == date.Value)))
                 .ReturnsAsync(new List<InterestRule> { existing.Value });

        var result = await _service.AddInterestRuleAsync(dateStr, ruleId, rate);

        Assert.True(result.IsSuccess);
        _repoMock.Verify(r => r.RemoveInterestRuleByDateAsync(It.IsAny<BusinessDate>()), Times.Once);
        _repoMock.Verify(r => r.SaveAsync(It.IsAny<InterestRule>()), Times.Once);
    }

    [Fact]
    public async Task AddInterestRuleAsync_Should_Fail_For_Invalid_Rate()
    {
        var result = await _service.AddInterestRuleAsync("20230615", "RULE01", 120m);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InvalidInterestRateAmount, result.Error);
    }

    [Fact]
    public async Task GetAllInterestRulesAsync_Should_Return_Dtos()
    {
        var rule1Result = InterestRule.Create(BusinessDate.From("20230101"), "RULE01", 1.95m);
        var rule2Result = InterestRule.Create(BusinessDate.From("20230615"), "RULE03", 2.20m);

        Assert.NotNull(rule1Result.Value);
        Assert.NotNull(rule2Result.Value);

        var rules = new List<InterestRule>
        {
            rule1Result.Value,
            rule2Result.Value
        };

        _repoMock.Setup(r => r.GetAllInterestRulesAsync())
                 .ReturnsAsync(rules);

        var result = await _service.GetAllInterestRulesAsync();

        Assert.True(result.IsSuccess);
        var ordered = result.Value.ToList();
        Assert.Equal("20230101", ordered[0].EffectiveDate);
        Assert.Equal("20230615", ordered[1].EffectiveDate);
    }

    [Fact]
    public async Task GetAllInterestRulesAsync_Should_Return_Empty_If_None()
    {
        _repoMock.Setup(r => r.GetAllInterestRulesAsync())
                 .ReturnsAsync(new List<InterestRule>());

        var result = await _service.GetAllInterestRulesAsync();

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}