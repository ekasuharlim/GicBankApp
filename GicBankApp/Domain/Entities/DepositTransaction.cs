namespace GicBankApp.Domain.Entities;

using GicBankApp.Domain.ValueObjects;
public class DepositTransaction : Transaction
{
    public DepositTransaction(
        BusinessDate date, 
        TransactionId transactionId, 
        Money amount) : base(date, transactionId, TransactionType.Deposit, amount)
    {
    }
    
    public override Money GetBalance(Money latestBalance)
    {
        return latestBalance + Amount;
    }
}