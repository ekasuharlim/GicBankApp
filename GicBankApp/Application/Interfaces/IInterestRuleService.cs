namespace GicBankApp.Application.Interfaces;

using GicBankApp.Shared;
using GicBankApp.Application.Dtos;
public interface IInterestRuleService
{
    Task<Result<InterestRuleDto>> AddInterestRuleAsync(string dateStr, string ruleId, decimal rate);

    Task<Result<IEnumerable<InterestRuleDto>>> GetAllInterestRulesAsync();

}
