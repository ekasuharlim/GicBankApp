namespace GicBankApp.Domain.ValueObjects
{
    public class InterestPeriod
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public decimal RatePercentage { get; }

        public decimal EodBalance {get;}

        public InterestPeriod(
            DateTime startDate, DateTime endDate, decimal ratePercentage, decimal eodBalance)
        {
            StartDate = startDate;
            EndDate = endDate;
            RatePercentage = ratePercentage;
            EodBalance = eodBalance;
        }

        public int NumberOfDays => (EndDate - StartDate).Days + 1;
        public decimal CalculateInterest()
        {
            
            return EodBalance * (RatePercentage / 100) * NumberOfDays;
        }

    }
}