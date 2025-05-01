namespace GicBankApp.Infrastructure.Services;

using GicBankApp.Domain.Common;
using GicBankApp.Domain.ValueObjects;

public class TransactionIdGenerator : ITransactionIdGenerator
{
    private readonly Dictionary<string, int> _dailySequence = new();

    public TransactionId GenerateId(BusinessDate date)
    {
        var key = date.ToString(); 

        if (!_dailySequence.ContainsKey(key))
            _dailySequence[key] = 1;
        else
            _dailySequence[key]++;

        var sequence = _dailySequence[key]; 
        return new TransactionId(date, sequence);
    }
}
