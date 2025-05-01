namespace GicBankApp.Domain.ValueObjects;
public sealed class Money
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        if (value < 0 || decimal.Round(value, 2) != value)
            throw new ArgumentException("Money must be >= 0 and have up to 2 decimal places.");
        Value = value;
    }

    public static Money operator +(Money a, Money b) => new Money(a.Value + b.Value);
    public static Money operator -(Money a, Money b)
    {
        var result = a.Value - b.Value;
        if (result < 0) throw new InvalidOperationException("Money cannot go below zero.");
        return new Money(result);
    }

    public override string ToString() => Value.ToString("0.00");
}