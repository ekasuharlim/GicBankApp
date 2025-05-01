namespace GicBankApp.Domain.Aggregates;

using GicBankApp.Domain.Common;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;

public class BankAccount : Entity, IAggregateRoot
{
    private readonly List<Transaction> _transactions = new();

    private Money _latestBalance = new Money(0);

    public BankAccount(string accountId)
    {
        AccountId = accountId;
    }

    public string AccountId { get; private set; }

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    public Result<Transaction> AddTransaction(Transaction transaction){
        _transactions.Add(transaction);
        return Result<Transaction>.Success(transaction);
    }
    public Result<decimal> GetBalanceUpTo(DateTime date) {
        return Result<decimal>.Success(0);
    }
}