namespace GicBankApp.Domain.Aggregates;

using System.Threading.Tasks;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetByIdAsync(string id);
    Task SaveAsync(BankAccount bankAccount);
}
