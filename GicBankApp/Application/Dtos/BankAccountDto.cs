namespace GicBankApp.Application.Dtos;
public class BankAccountDto 
{
        public string AccountId { get; set; }
        public decimal LatestBalance { get; set; }
        public List<TransactionDto> Transactions { get; set; }

        public BankAccountDto()
        {
            Transactions = new List<TransactionDto>();
            AccountId = string.Empty;
        }

        public BankAccountDto(string accountId, decimal latestBalance, List<TransactionDto> transactions)
        {
            AccountId = accountId;
            LatestBalance = latestBalance;
            Transactions = transactions;
        }    
}