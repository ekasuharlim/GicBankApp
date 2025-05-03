namespace GicBankApp.Application.Interfaces;

using GicBankApp.Domain.ValueObjects;
using GicBankApp.Domain.Aggregates;

public interface IEodBalanceService
{
    Dictionary<DateTime, decimal> CalculateEodBalances(
        BankAccount account,
        DateTime startDate, 
        DateTime endDate);
}