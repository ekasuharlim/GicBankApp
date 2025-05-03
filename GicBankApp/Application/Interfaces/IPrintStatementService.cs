namespace GicBankApp.Application.Interfaces;
using GicBankApp.Application.Dtos;
using GicBankApp.Shared;
public interface IPrintStatementService
{
    Task<Result<AccountStatementDto>> PrintStatementAsync(string accountNumber, int year , int month);
    
}