namespace GicBankApp.Domain.Factories;

using GicBankApp.Domain.Common;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.ValueObjects;
public class TransactionFactory : ITransactionFactory
{
    private readonly ITransactionIdGenerator _transactionIdGenerator;

    public TransactionFactory(ITransactionIdGenerator transactionIdGenerator)
    {
        _transactionIdGenerator = transactionIdGenerator;
    }

    public Transaction CreateTransaction(
        BusinessDate date, 
        Money amount,
        string transactionType)
    {
        var transactionId = _transactionIdGenerator.GenerateId(date);
        switch(transactionType.ToUpperInvariant())
        {
            case "D":
                return new DepositTransaction(date, transactionId, amount);
            case "W":
                return new WithdrawalTransaction(date, transactionId, amount);
            default:
                throw new ArgumentException("Invalid transaction type");
        }
        
    }
}