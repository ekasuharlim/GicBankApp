namespace GicBankApp.Tests.Domain.ValueObjects;


using GicBankApp.Domain.ValueObjects;
public class BusinessDateTests
{
    [Fact]
    public void From_DateTime_Should_Create_BusinessDate()
    {
        var dateTime = new DateTime(2023, 6, 26);

        var businessDate = BusinessDate.From(dateTime);

            Assert.Equal(dateTime.Date, businessDate.Value);
    }

    [Fact]
    public void From_String_ValidFormat_Should_Create_BusinessDate()
    {
        var businessDate = BusinessDate.From("20230626");

        Assert.Equal(new DateTime(2023, 6, 26), businessDate.Value);
    }

    [Fact]
    public void From_String_InvalidFormat_Should_ThrowArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => BusinessDate.From("26-06-2023"));
        Assert.Equal("Invalid date format. Expected yyyyMMdd.", exception.Message);
    }

}