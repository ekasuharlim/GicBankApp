namespace GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;

public interface IInterestRuleRepository
{
    Task<IEnumerable<InterestRule>> GetInterestRules(int month, int year);

    Task<IEnumerable<InterestRule>> GetAllInterestRules();

    Task SaveAsync(InterestRule interestRule);
}