namespace GicBankApp.Application.Mappers;
using GicBankApp.Application.Dtos;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.ValueObjects;

public static class BankAccountMapper
{
    public static BankAccountDto ToDto(BankAccount account)
    {
        return new BankAccountDto
        {
            AccountId = account.AccountId,
            LatestBalance = account.LatestBalance.Value,
            Transactions = account.Transactions
                .Select(TransactionMapper.ToDto)
                .OrderBy(t => t.Date)
                .ToList()
        };
    }
}