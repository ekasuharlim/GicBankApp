using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.Common;
using GicBankApp.Application.DTOs;
using GicBankApp.Application.Interfaces;
using GicBankApp.Domain.Factories;
using GicBankApp.Shared;
using GicBankApp.Domain.ValueObjects;
public class TransactionService : ITransactionService
{
    private readonly IBankAccountRepository _accountRepo;
    private readonly ITransactionIdGenerator _idGenerator;

    private readonly ITransactionFactory _transactionFactory;

    public TransactionService(
        IBankAccountRepository accountRepo, 
        ITransactionIdGenerator idGenerator,
        ITransactionFactory transactionFactory)
    {
        _accountRepo = accountRepo;
        _idGenerator = idGenerator;
        _transactionFactory = transactionFactory;
    }

    public async Task<Result<TransactionDTO>> AddTransactionAsync(
        string date, string accountId, string type, decimal amount)
    {
        var existingAccount = await _accountRepo.GetByIdAsync(accountId);
        var account = existingAccount ?? new BankAccount(accountId);

        var transaction = _transactionFactory.CreateTransaction(
            BusinessDate.From(date), 
            new Money(amount), 
            type);        
        
        account.AddTransaction(transaction); 

        _accountRepo.Save(account);

        var transactions = account.GetTransactions();  // from Domain
        var latest = transactions.Last();

        return new TransactionDTO
        {
            TransactionId = latest.TransactionId,
            AccountId = latest.AccountId,
            Amount = latest.Amount.Value,
            Type = latest.Type.ToString(),
            Date = latest.Date.ToString("yyyy-MM-dd")
        };
    }

}
