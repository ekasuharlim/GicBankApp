namespace GicBankApp.Application.Mappers;

using GicBankApp.Application.Dtos;
using GicBankApp.Domain.Entities;
using GicBankApp.Shared;

public static class InterestRuleMapper
{
    public static InterestRuleDto ToDto(InterestRule entity)
    {
        return new InterestRuleDto(
            effectiveDate: entity.EffectiveDate.ToString(), 
            ruleId: entity.RuleId,
            ratePercent: entity.RatePercentage
        );
    }

}