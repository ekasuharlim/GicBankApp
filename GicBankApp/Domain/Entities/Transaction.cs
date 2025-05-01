namespace GicBankApp.Domain.Entities;

using GicBankApp.Domain.Common;
using GicBankApp.Domain.ValueObjects;

public abstract class Transaction : Entity
{
    public BusinessDate Date { get; }
    public TransactionId TransactionId { get; }
    public TransactionType Type { get; }
    public Money Amount { get; }

    public abstract Money GetBalance(Money latestBalance);


    protected Transaction(
        BusinessDate date, 
        TransactionId transactionId, 
        TransactionType type, 
        Money amount){

        Date = date;
        TransactionId = transactionId;
        Type = type;
        Amount = amount;
    }

}