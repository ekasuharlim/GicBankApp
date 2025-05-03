namespace GicBankApp.Application.Interfaces;

using GicBankApp.Domain.ValueObjects;
using GicBankApp.Domain.Aggregates;

public interface IInterestCalculatorService
{
    Task<decimal> CalculateTotalInterestAsync(BankAccount account, MonthlyPeriod period);

}