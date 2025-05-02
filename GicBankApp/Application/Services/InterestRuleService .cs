namespace GicBankApp.Application.Services;

using GicBankApp.Application.Interfaces;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;
using GicBankApp.Application.Dtos;
using GicBankApp.Application.Mappers;
using System.Collections.Generic;

public class InterestRuleService : IInterestRuleService
{
    private readonly IInterestRuleRepository _interestRuleRepo;


    public InterestRuleService(
        IInterestRuleRepository interestRuleRepo)
    {
        _interestRuleRepo = interestRuleRepo;
    }

    public async Task<Result<InterestRuleDto>> AddInterestRuleAsync(string dateStr, string ruleId, decimal rate)
    {
        var date = BusinessDate.From(dateStr);
        var rateResult = InterestRule.Create(date, ruleId, rate);
        if (!rateResult.IsSuccess)
        {
            return Result<InterestRuleDto>.Failure(rateResult.Error);
        }
        var interestRule = rateResult.Value;    

        var existingRules = await _interestRuleRepo.GetInterestRuleByDateAsync(date);
        if (existingRules != null && existingRules.Count > 0)
        {
            await _interestRuleRepo.RemoveInterestRuleByDateAsync(date);
        }

        await _interestRuleRepo.SaveAsync(interestRule);

        return Result<InterestRuleDto>.Success(InterestRuleMapper.ToDto(interestRule));
    }

    public async Task<Result<IEnumerable<InterestRuleDto>>> GetAllInterestRulesAsync()
    {
        var rules = await _interestRuleRepo.GetAllInterestRulesAsync();

        if (rules is null || !rules.Any())
        {
            return Result<IEnumerable<InterestRuleDto>>.Success(Enumerable.Empty<InterestRuleDto>());
        }

        var dtos = rules
            .Select(InterestRuleMapper.ToDto);

        return Result<IEnumerable<InterestRuleDto>>.Success(dtos);    
    }
}