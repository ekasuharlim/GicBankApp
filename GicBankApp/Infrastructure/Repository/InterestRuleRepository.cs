using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;

public class InterestRuleRepository : IInterestRuleRepository
{
    private readonly List<InterestRule> _interestRules = new List<InterestRule>();

    public Task<IEnumerable<InterestRule>> GetAllInterestRulesAsync()
    {
        return Task.FromResult(_interestRules.AsEnumerable());
    }

    public Task<IReadOnlyList<InterestRule>> GetInterestRuleByDateAsync(BusinessDate date)
    {
        var result = _interestRules
            .Where(r => r.EffectiveDate.Equals(date))
            .ToList()
            .AsReadOnly();
        return Task.FromResult<IReadOnlyList<InterestRule>>(result);
    }

    public Task<IReadOnlyList<InterestRule>> RemoveInterestRuleByDateAsync(BusinessDate date)
    {
        _interestRules.RemoveAll(r => r.EffectiveDate.Equals(date));
        return Task.FromResult<IReadOnlyList<InterestRule>>(_interestRules.AsReadOnly());    
    }

    public Task SaveAsync(InterestRule interestRule)
    {
            _interestRules.Add(interestRule);
            return Task.CompletedTask;
    }
} 