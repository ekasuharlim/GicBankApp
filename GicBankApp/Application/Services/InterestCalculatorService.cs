namespace GicBankApp.Application.Services;

using GicBankApp.Application.Interfaces;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Domain.Entities;

public class InterestCalculatorService : IInterestCalculatorService {

    private readonly IInterestRuleRepository _interestRuleRepository;
    private readonly IEodBalanceService _eodBalanceService;

    public InterestCalculatorService(
        IInterestRuleRepository interestRuleRepository,
        IEodBalanceService eodBalanceService)
    {
        _interestRuleRepository = interestRuleRepository;
        _eodBalanceService = eodBalanceService;
    }

    public async Task<decimal> CalculateTotalInterestAsync(
        BankAccount account, MonthlyPeriod period)
    {
        DateTime startDate = period.StartDate;
        DateTime endDate = period.EndDate;

        Dictionary<DateTime, decimal> eodBalances = 
        _eodBalanceService.CalculateEodBalances(account, startDate, endDate);

        decimal previousPeriodBalance = 
            account.GetBalanceBeforeDate(BusinessDate.From(startDate)).Value;

        var interestRules = await _interestRuleRepository.GetAllInterestRulesAsync();

        IEnumerable<InterestPeriod> interestPeriods = GetInterestPeriods(
            previousPeriodBalance,
            eodBalances,
            interestRules,
            startDate,
            endDate
        );        

        decimal totalInterest = 0;

        foreach(var interestPeriod in interestPeriods) {
            totalInterest = totalInterest + interestPeriod.CalculateInterest();
        }

        return totalInterest / 365;
    }

    private List<InterestPeriod> GetInterestPeriods(
        decimal previousPeriodBalance,
        Dictionary<DateTime, decimal> eodBalance, 
        IEnumerable<InterestRule> allRules, 
        DateTime startDate, DateTime endDate)
    {
        var result = new List<InterestPeriod>();

        var activeRules = allRules
            .Where(r => r.EffectiveDate.Value <= endDate)
            .OrderBy(r => r.EffectiveDate.Value)
            .ToList();
        
        var latestRuleBeforeThisPeriod = 
            allRules.FirstOrDefault(r => r.EffectiveDate.Value <= startDate);
        
        decimal latestRate = 
            latestRuleBeforeThisPeriod != null ? 
            latestRuleBeforeThisPeriod.RatePercentage: 
            0.0m;

        decimal latestEodBalance = previousPeriodBalance;
        DateTime currentStartDate = startDate;
        
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            bool foundNewBalance =  eodBalance.ContainsKey(date);
            bool foundNewRate = activeRules.Any(r => r.EffectiveDate.Value.Equals(date));

            if (!(foundNewBalance || foundNewRate)) continue;

        
            if(!date.Equals(startDate)) {
                result.Add(new InterestPeriod(
                    currentStartDate, 
                    date.AddDays(-1), 
                    latestRate, 
                    latestEodBalance
                ));
            }

            currentStartDate = date;

            if (foundNewBalance) {
                latestEodBalance = eodBalance[date];
            }
            if (foundNewRate) {
                latestRate = activeRules.First(r => r.EffectiveDate.Value.Equals(date)).RatePercentage;
            }            
        }
        result.Add(new InterestPeriod(
            currentStartDate, 
            endDate, 
            latestRate, 
            latestEodBalance
        ));

        return result;    
    }    

}