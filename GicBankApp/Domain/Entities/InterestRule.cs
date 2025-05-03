using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;

namespace GicBankApp.Domain.Entities;

public class InterestRule
{
    public BusinessDate EffectiveDate { get; private set; }
    public string RuleId { get; private set; }
    public decimal RatePercentage { get; private set; }

    private InterestRule(BusinessDate date, string ruleId, decimal ratePercentage) {
        EffectiveDate = date;
        RuleId = ruleId;
        RatePercentage = ratePercentage;
    }

    public static Result<InterestRule> Create(BusinessDate date, string ruleId, 
        decimal ratePercentage)
    {
        if (ratePercentage <= 0 || ratePercentage >= 100)
            return Result<InterestRule>.Failure(Error.InvalidInterestRateAmount);

        return Result<InterestRule>.Success(new InterestRule(date, ruleId, ratePercentage));
    }
}