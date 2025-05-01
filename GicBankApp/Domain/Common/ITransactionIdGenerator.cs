namespace GicBankApp.Domain.Common;
using GicBankApp.Domain.ValueObjects;
public interface ITransactionIdGenerator
{
    string GenerateId(BusinessDate date);
}
