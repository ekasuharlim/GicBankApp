namespace GicBankApp.Application.Mappers;
using GicBankApp.Application.Dtos;
using GicBankApp.Domain.Entities;

public static class TransactionMapper
{
    public static TransactionDto ToDto(Transaction transaction)
    {
        return new TransactionDto
        {
            Date = transaction.Date.ToString(),
            TransactionId = transaction.TransactionId.Value,
            Type = transaction.Type.ToString().Substring(0, 1).ToUpperInvariant(),
            Amount = transaction.Amount.Value
        };
    }
}