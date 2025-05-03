namespace GicBankApp.Domain.ValueObjects;

using GicBankApp.Shared;
public sealed class MonthlyPeriod : IEquatable<MonthlyPeriod>
{
    public int Year { get; }
    public int Month { get; }

    public DateTime StartDate { get; private set;}
    public DateTime EndDate { get; private set;}

    private MonthlyPeriod(int year, int month)
    {
        Year = year;
        Month = month;

        StartDate = new DateTime(year, month, 1);
        EndDate = StartDate.AddMonths(1).AddDays(-1);        
    }

    public static Result<MonthlyPeriod> Create(int year, int month)
    {
        if (month < 1 || month > 12)
            return Result<MonthlyPeriod>.Failure(Error.InvalidMonthlyPeriodMonth);

        return Result<MonthlyPeriod>.Success(new MonthlyPeriod(year, month));
    }

    public override string ToString() => $"{Year}{Month:D2}";

    public bool Equals(MonthlyPeriod? other)
    {
        return other is not null && Year == other.Year && Month == other.Month;
    }

}