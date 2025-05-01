namespace GicBankApp.Domain.Entities;

using GicBankApp.Domain.ValueObjects;
public class WithdrawalTransaction : Transaction
{
    public WithdrawalTransaction(
        BusinessDate date, 
        TransactionId transactionId, 
        Money amount) : base(date, transactionId, TransactionType.Withdrawal, amount)
    {
    }
    
    public override Money GetBalance(Money latestBalance)
    {
        return latestBalance - Amount;
    }
}