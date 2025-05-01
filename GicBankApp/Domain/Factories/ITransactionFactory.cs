namespace GicBankApp.Domain.Factories;

using GicBankApp.Domain.ValueObjects;
using GicBankApp.Domain.Entities;

public interface ITransactionFactory
{
    public Transaction CreateTransaction(BusinessDate date, Money amount, string transactionType);

}