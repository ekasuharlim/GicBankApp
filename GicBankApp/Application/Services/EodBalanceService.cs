namespace GicBankApp.Application.Services;

using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Application.Interfaces;

public class EodBalanceService : IEodBalanceService
{
    public Dictionary<DateTime, decimal> CalculateEodBalances(
        BankAccount account,
        DateTime startDate, DateTime endDate)
    {
        var eodBalances = new Dictionary<DateTime, decimal>();
        Money runningBalance = account.GetBalanceBeforeDate(BusinessDate.From(startDate));

        var transactionBydate = account.Transactions
            .Where(t => t.Date.Value >= startDate && t.Date.Value <= endDate)
            .GroupBy(t => t.Date.Value)
            .ToDictionary(g => g.Key, g => g.ToList());
        
        foreach (var date in transactionBydate.Keys)
        {
            foreach(var transaction in transactionBydate[date])
            {
                runningBalance = transaction.GetBalance(runningBalance);
            }
            eodBalances[date] = runningBalance.Value;
        }
        return eodBalances;
    }

}