namespace GicBankApp.Tests.Domain.ValueObjects;

using GicBankApp.Domain.ValueObjects;
public class InterestPeriodTest
{
    [Fact]
    public void CalculateInterest_ShouldReturnCorrectInterest()
    {
        var startDate = new DateTime(2023, 6, 1);
        var endDate = new DateTime(2023, 6, 14);
        var rate = 1.9m; 
        var eodBalance = 250m;

        var interestPeriod = new InterestPeriod(startDate, endDate, rate, eodBalance);

        var interest = interestPeriod.CalculateInterest();

        Assert.Equal(66.5m, interest);
   }

   [Fact]
   public void NumberOfDays_ShouldReturnCorrectNumberOfDays()
   {
       var startDate = new DateTime(2023, 6, 1);
       var endDate = new DateTime(2023, 6, 14);

       var interestPeriod = new InterestPeriod(startDate, endDate, 0.05m, 250m);

       Assert.Equal(14, interestPeriod.NumberOfDays);
   }
}