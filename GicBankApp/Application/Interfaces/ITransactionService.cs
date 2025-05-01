namespace GicBankApp.Application.Interfaces;

using GicBankApp.Application.DTOs;
using GicBankApp.Shared;
public interface ITransactionService
{
    Task<Result<TransactionDTO>> AddTransactionAsync(DateTime date, string accountId, string type, decimal amount);

}