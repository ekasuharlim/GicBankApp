namespace GicBankApp.Domain.Aggregates;

using GicBankApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetByIdAsync(string id);
    Task SaveAsync(BankAccount bankAccount);
}
