namespace GicBankApp.Domain.UnitTests.ValueObjects;

using GicBankApp.Shared;
using GicBankApp.Domain.ValueObjects;

public class MonthlyPeriodTests
{
    [Fact]
    public void Create_ValidMonthAndYear_ReturnsSucces()
    {
        int year = 2025;
        int month = 5;

        Result<MonthlyPeriod> result = MonthlyPeriod.Create(year, month);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(year, result.Value.Year);
        Assert.Equal(month, result.Value.Month);
        Assert.Null(result.Error);
    }

    [Theory]
    [InlineData(2025, 0)]
    [InlineData(2025, 13)]
    [InlineData(2024, -1)]
    public void Create_InvalidMonth_ReturnsFailureWithError(int year, int month)
    {

        Result<MonthlyPeriod> result = MonthlyPeriod.Create(year, month);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.NotNull(result.Error);
        Assert.Equal(Error.InvalidMonthlyPeriodMonth, result.Error);
    }


    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        int year = 2025;
        int month = 5;
        var result = MonthlyPeriod.Create(year, month);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("202505", result.Value.ToString());

    }

    [Fact]
    public void Equals_SameYearAndMonth_ReturnsTrue()
    {
        var period1Result = MonthlyPeriod.Create(2025, 5);
        var period2Result = MonthlyPeriod.Create(2025, 5);

        Assert.True(period1Result.IsSuccess);
        Assert.True(period2Result.IsSuccess);

        Assert.NotNull(period1Result.Value);
        Assert.NotNull(period2Result.Value);
        Assert.True(period1Result.Value.Equals(period2Result.Value));
    }

    [Theory]
    [InlineData(2025, 6)]
    [InlineData(2026, 5)]
    [InlineData(2023, 4)]
    public void Equals_DifferentPeriod_ReturnsFalse(int year2, int month2)
    {
        var period1 = MonthlyPeriod.Create(2025, 5);
        var period2 = MonthlyPeriod.Create(year2, month2);

        Assert.True(period1.IsSuccess);
        Assert.True(period2.IsSuccess);

        Assert.NotNull(period1.Value);
        Assert.NotNull(period2.Value);
        Assert.False(period1.Value.Equals(period2.Value));
    }

}