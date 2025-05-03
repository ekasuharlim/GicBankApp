namespace GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;

public interface IInterestRuleRepository
{
    Task<IEnumerable<InterestRule>> GetAllInterestRulesAsync();

    Task SaveAsync(InterestRule interestRule);
    Task<IReadOnlyList<InterestRule>> GetInterestRuleByDateAsync(BusinessDate date);
    Task<IReadOnlyList<InterestRule>> RemoveInterestRuleByDateAsync(BusinessDate date);

}