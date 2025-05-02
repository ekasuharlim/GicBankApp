namespace GicBankApp.Application.Dtos;
public class TransactionDto
{
    public string Date { get; set; }          
    public string TransactionId { get; set; }  
    public string Type { get; set; }           
    public decimal Amount { get; set; }        
    public decimal Balance { get; set; }       

    public TransactionDto()
    {
        Date = string.Empty;
        TransactionId = string.Empty;
        Type = string.Empty;
        Amount = 0;
        Balance = 0;
    }

    public TransactionDto(string date, string transactionId, string type, decimal amount, decimal balance = 0)
    {
        Date = date;
        TransactionId = transactionId;
        Type = type;
        Amount = amount;
        Balance = balance;
    }
}