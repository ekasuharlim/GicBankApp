namespace GicBankApp.Application.Dtos;
public class AccountStatementDto 
{
        public string AccountId { get; set; }
        public decimal LatestBalance { get; set; }
        public List<TransactionDto> Transactions { get; set; }

        public AccountStatementDto()
        {
            Transactions = new List<TransactionDto>();
            AccountId = string.Empty;
        }

        public AccountStatementDto(string accountId, decimal latestBalance, List<TransactionDto> transactions)
        {
            AccountId = accountId;
            LatestBalance = latestBalance;
            Transactions = transactions;
        }    
}