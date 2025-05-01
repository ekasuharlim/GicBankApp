namespace GicBankApp.Domain.ValueObjects;
public sealed class Money
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        if (decimal.Round(value, 2) != value)
            throw new ArgumentException("Money must have up to 2 decimal places.");
        Value = value;
    }

    public static Money operator +(Money a, Money b) => new Money(a.Value + b.Value);
    public static Money operator -(Money a, Money b)
    {
        var result = a.Value - b.Value;
        return new Money(result);
    }

    public override string ToString() => Value.ToString("0.00");
}