namespace GicBankApp.Domain.ValueObjects;

public sealed class TransactionId
{
    public string Value { get; }

    public TransactionId(BusinessDate date, int sequence)
    {
        Value = $"{date}-{sequence:D2}";
    }

    public override string ToString() => Value;
}