namespace GicBankApp.Tests.Application.Services;

using GicBankApp.Application.Services;
using GicBankApp.Application.Interfaces;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Domain.Entities;
using GicBankApp.Infrastructure.Services;
using GicBankApp.Domain.Factories;

public class InterestCalculatorServiceTests
{
    private readonly Mock<IInterestRuleRepository> _mockInterestRuleRepo;
    private readonly Mock<IEodBalanceService> _mockEodBalanceService;
    private readonly InterestCalculatorService _service;
    private readonly TransactionFactory _transactionFactory;

    public InterestCalculatorServiceTests()
    {
        _mockInterestRuleRepo = new Mock<IInterestRuleRepository>();
        _mockEodBalanceService = new Mock<IEodBalanceService>();
        _service = new InterestCalculatorService(_mockInterestRuleRepo.Object, _mockEodBalanceService.Object);
        _transactionFactory = new TransactionFactory(new TransactionIdGenerator());
    }

    [Fact]
    public async Task CalculateTotalInterestAsync_CalculatesCorrectInterest_ForGivenPeriod()
    {
        var account = new BankAccount("AC001");
        var period = MonthlyPeriod.Create(2023, 6).Value;

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230505"), new Money(100.00m), "D"));
        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230601"), new Money(150.00m), "D"));
        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230626"), new Money(20.00m), "W"));
        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20230626"), new Money(100.00m), "W"));

        var eodBalances = new Dictionary<DateTime, decimal>
        {
            [new DateTime(2023, 6, 1)] = 250.00m,
            [new DateTime(2023, 6, 26)] = 130.00m
        };

        Assert.NotNull(period);
        _mockEodBalanceService
            .Setup(x => x.CalculateEodBalances(account, period.StartDate, period.EndDate))
            .Returns(eodBalances);

        var rules = new List<InterestRule>
        {
            InterestRule.Create(BusinessDate.From("20230101"), "RULE01", 1.95m).Value!,
            InterestRule.Create(BusinessDate.From("20230520"), "RULE02", 1.90m).Value!,
            InterestRule.Create(BusinessDate.From("20230615"), "RULE03", 2.20m).Value!
        };

        _mockInterestRuleRepo
            .Setup(r => r.GetAllInterestRulesAsync())
            .ReturnsAsync(rules);

        var result = await _service.CalculateTotalInterestAsync(account, period);

        Assert.Equal(0.39m, Math.Round(result, 2));
    }

    [Fact]
    public async Task CalculateTotalInterestAsync_ShouldHaveZeroInterestRateIfNoRateSpecifiedBeforeThePeriod()
    {
        var account = new BankAccount("AC002");
        var period = MonthlyPeriod.Create(2024, 1).Value;

        account.AddTransaction(_transactionFactory.CreateTransaction(
            BusinessDate.From("20231231"), new Money(1000m), "D"));

        var eodBalances = new Dictionary<DateTime, decimal>
        {
            [new DateTime(2024, 1, 1)] = 1000m,
            [new DateTime(2024, 1, 15)] = 1000m,
            [new DateTime(2024, 1, 31)] = 1000m
        };

        _mockEodBalanceService
            .Setup(x => x.CalculateEodBalances(account, period.StartDate, period.EndDate))
            .Returns(eodBalances);

        var rules = new List<InterestRule>
        {
            InterestRule.Create(BusinessDate.From("20240210"), "RULE01", 2.0m).Value!,
            InterestRule.Create(BusinessDate.From("20240220"), "RULE02", 2.5m).Value!
        };

        _mockInterestRuleRepo
            .Setup(r => r.GetAllInterestRulesAsync())
            .ReturnsAsync(rules);

        var result = await _service.CalculateTotalInterestAsync(account, period);
        Console.WriteLine(result);
        Assert.Equal(0, result);    
    }

}