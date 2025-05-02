namespace GicBankApp.Application.Services;

using GicBankApp.Application.Dtos;
using GicBankApp.Application.Interfaces;
using GicBankApp.Domain.Factories;
using GicBankApp.Shared;
using GicBankApp.Domain.Common;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Application.Mappers;
using GicBankApp.Domain.Entities;

public class TransactionService : ITransactionService
{
    private readonly IBankAccountRepository _accountRepo;

    private readonly ITransactionFactory _transactionFactory;

    public TransactionService(
        IBankAccountRepository accountRepo, 
        ITransactionFactory transactionFactory)
    {
        _accountRepo = accountRepo;
        _transactionFactory = transactionFactory;
    }

    public async Task<Result<BankAccountDto>> AddTransactionAsync(
        string date, string accountId, string type, decimal amount)
    {
        var existingAccount = await _accountRepo.GetByIdAsync(accountId);
        var account = existingAccount ?? new BankAccount(accountId);

        var transaction = _transactionFactory.CreateTransaction(
            BusinessDate.From(date), 
            new Money(amount), 
            type);        
        
        Result<Transaction> result = account.AddTransaction(transaction); 
        if(!result.IsSuccess){
            return Result<BankAccountDto>.Failure(result.Error);
        }

        await _accountRepo.SaveAsync(account);

        return Result<BankAccountDto>.Success(
            BankAccountMapper.ToDto(account));

    }


}
