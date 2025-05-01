namespace GicBankApp.Tests.Infrastructure.Services;

using GicBankApp.Domain.ValueObjects;
using GicBankApp.Infrastructure.Services;

public class TransactionIdGeneratorTests
{
    [Fact]
    public void GenerateId_ShouldReturnCorrectFormat_OnFirstCallForDate()
    {
        var generator = new TransactionIdGenerator();
        var date = BusinessDate.From("20230626");

        var id = generator.GenerateId(date);

        Assert.Equal("20230626-01", id);
    }

    [Fact]
    public void GenerateId_ShouldIncrementSequence_ForSameDate()
    {
        var generator = new TransactionIdGenerator();
        var date = BusinessDate.From("20230626");

        var id1 = generator.GenerateId(date);
        var id2 = generator.GenerateId(date);
        var id3 = generator.GenerateId(date);

        Assert.Equal("20230626-01", id1);
        Assert.Equal("20230626-02", id2);
        Assert.Equal("20230626-03", id3);
    }

    [Fact]
    public void GenerateId_ShouldResetSequence_ForDifferentDates()
    {
        var generator = new TransactionIdGenerator();
        var date1 = BusinessDate.From("20230626");
        var date2 = BusinessDate.From("20230627");

        var id1 = generator.GenerateId(date1);
        var id2 = generator.GenerateId(date2);
        var id3 = generator.GenerateId(date1);

        Assert.Equal("20230626-01", id1);
        Assert.Equal("20230627-01", id2);
        Assert.Equal("20230626-02", id3);
    }
}
