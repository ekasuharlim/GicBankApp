namespace GicBankApp.Domain.Aggregates;

using GicBankApp.Domain.Common;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Shared;

public class BankAccount : Entity, IAggregateRoot
{
    private readonly List<Transaction> _transactions = new();


    public BankAccount(string accountId)
    {
        AccountId = accountId;
        LatestBalance = new Money(0);
    }

    public string AccountId { get; private set; }

    public Money LatestBalance { get; private set;}

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    public Result<Transaction> AddTransaction(Transaction transaction){
        if (_transactions.Count == 0 && transaction.Type == TransactionType.Withdrawal)
        {
            return Result<Transaction>.Failure(Error.FirstTransactionCannotBeWithdrawal);
        }

        if (transaction.Type == TransactionType.Withdrawal)
        {
            var newBalance = transaction.GetBalance(LatestBalance);
            if(newBalance.Value < 0)
            {
                return Result<Transaction>.Failure(Error.InsufficentBalance);

            }
        }

        LatestBalance = transaction.GetBalance(LatestBalance);
        _transactions.Add(transaction);
        return Result<Transaction>.Success(transaction);
    }
}