namespace GicBankApp.Infrastructure.Repository;

using GicBankApp.Domain.Aggregates;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly List<BankAccount> _bankAccounts = new List<BankAccount>();

    public Task<BankAccount?> GetByIdAsync(string accountId)
    {
        var bankAccount = _bankAccounts.FirstOrDefault(b => b.AccountId == accountId);
        return Task.FromResult(bankAccount);
    }

    public Task SaveAsync(BankAccount bankAccount)
    {
        _bankAccounts.Add(bankAccount);
        return Task.CompletedTask;
    }
} 