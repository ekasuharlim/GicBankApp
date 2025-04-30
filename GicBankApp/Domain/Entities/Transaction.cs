namespace GicBankApp.Domain.Entities;

using GicBankApp.Domain.Common;
using GicBankApp.Domain.ValueObjects;

public class Transaction : Entity
{
    public DateTime Date { get; }
    public string TransactionId { get; }
    public TransactionType Type { get; }
    public decimal Amount { get; }

    public Transaction(
        DateTime date, 
        string transactionId, 
        TransactionType type, 
        decimal amount){

        Date = date;
        TransactionId = transactionId;
        Type = type;
        Amount = amount;
    }
}