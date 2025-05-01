namespace GicBankApp.Application.Interfaces;

using GicBankApp.Application.Dtos;
using GicBankApp.Shared;
public interface ITransactionService
{
    Task<Result<BankAccountDto>> AddTransactionAsync(
        string date, string accountId, string type, decimal amount);

}