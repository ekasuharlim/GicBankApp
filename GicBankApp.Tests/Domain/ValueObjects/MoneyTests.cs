namespace GicBankApp.Tests.Domain.ValueObjects;

using GicBankApp.Domain.ValueObjects;

public class MoneyTests
{
    [Theory]
    [InlineData(-1)]  
    [InlineData(1.2345)] 
    public void Money_ShouldThrowException_ForInvalidAmount(decimal invalidMoney)
    {
        Assert.Throws<ArgumentException>(() => new Money(invalidMoney));
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(100.00)]
    [InlineData(0.99)]
    public void Money_ShouldNotThrowException_ForValidAmount(decimal validMoney)
    {
        var Money = new Money(validMoney);

        Assert.Equal(validMoney, Money.Value);
    }

    [Fact]
    public void Money_Addition_ShouldReturnCorrectResult()
    {
        var Money1 = new Money(10.00m);
        var Money2 = new Money(5.50m);

        var result = Money1 + Money2;

        Assert.Equal(15.50m, result.Value);
    }

    [Fact]
    public void Money_Subtraction_ShouldReturnCorrectResult()
    {
        var Money1 = new Money(10.00m);
        var Money2 = new Money(5.50m);

        var result = Money1 - Money2;

        Assert.Equal(4.50m, result.Value);
    }

 [Fact]
    public void Money_Subtraction_ShouldThrowExceptionForNegativeResult()
    {
        // Arrange
        var money1 = new Money(5.00m);
        var money2 = new Money(10.00m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => money1 - money2);
    }

}
