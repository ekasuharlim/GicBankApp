
namespace GicBankApp.Application.Services;

using System.Threading.Tasks;
using GicBankApp.Application.Dtos;
using GicBankApp.Application.Interfaces;
using GicBankApp.Domain.Aggregates;
using GicBankApp.Shared;

using GicBankApp.Domain.ValueObjects;
using GicBankApp.Application.Mappers;

public class PrintStatementService : IPrintStatementService
{

    private readonly IInterestCalculatorService _interestCalculatorService;
    private readonly IBankAccountRepository _accountRepo;
    public PrintStatementService(
        IBankAccountRepository accountRepo, 
        IInterestCalculatorService interestCalculatorService)
    {
        _accountRepo = accountRepo;
        _interestCalculatorService = interestCalculatorService;

    }

    public async Task<Result<AccountStatementDto>> PrintStatementAsync(string accountNumber, int year , int month)
    {
        //get account
        var account = await _accountRepo.GetByIdAsync(accountNumber);
        if (account == null)
        {
            return Result<AccountStatementDto>.Failure(Error.BankAccountNotFound);
        }
        var period = MonthlyPeriod.Create(year, month);
        if (!period.IsSuccess)
        {
            return Result<AccountStatementDto>.Failure(period.Error);
        }

        var reportPeriod = period.Value;


        List<TransactionDto> transactions = new List<TransactionDto>();
        //convert transactions      
        Money previousPeriodBalance = 
            account.GetBalanceBeforeDate(BusinessDate.From(reportPeriod.StartDate));
        
        Money runningBalance = previousPeriodBalance;
        

        foreach (var transaction in account.Transactions)
        {
            runningBalance = transaction.GetBalance(runningBalance);
            //convert transaction to dto

            var transactionDto = TransactionMapper.ToDto(transaction);
            transactionDto.Balance = runningBalance.Value;
            transactions.Add(transactionDto);            
        }

        //calculate interest amount 
        decimal monthlyInterest = 
            await _interestCalculatorService.CalculateTotalInterestAsync(account, reportPeriod);

        var interestTransactionDto = new TransactionDto
        {
            Date = BusinessDate.From(reportPeriod.EndDate).ToString(),
            Amount = monthlyInterest,
            TransactionId = String.Empty,
            Type = "I",
            Balance = runningBalance.Value
        };
        transactions.Add(interestTransactionDto);

        //create statement
        var statement = new AccountStatementDto
        {
            AccountId = account.AccountId,
            Transactions = transactions
        };
        
        return Result<AccountStatementDto>.Success(statement);
    }
}