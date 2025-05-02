namespace GicBankApp.Application.Dtos;
public sealed class InterestRuleDto
{
    public string EffectiveDate { get; set; } 
    public string RuleId { get; set; } 
    public decimal RatePercent { get; set; }

    public InterestRuleDto(string effectiveDate, string ruleId, decimal ratePercent)
    {
        EffectiveDate = effectiveDate;
        RuleId = ruleId;
        RatePercent = ratePercent;
    }

}