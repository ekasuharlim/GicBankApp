namespace GicBankApp.Domain.ValueObjects;

using GicBankApp.Domain.Common;

public sealed class TransactionId : ValueObject
{
    public string Value { get; }

    public TransactionId(BusinessDate date, int sequence)
    {
        Value = $"{date}-{sequence:D2}";
    }

    public override string ToString() => Value;
}