namespace GicBankApp.Domain.Common;
using GicBankApp.Domain.ValueObjects;
public interface ITransactionIdGenerator
{
    TransactionId GenerateId(BusinessDate date);
}
