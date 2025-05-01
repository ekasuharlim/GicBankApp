namespace GicBankApp.Application.DTOs;
public class TransactionDTO
{
    public string Date { get; set; }
    public string TransactionId { get; set; }
    public string Type { get; set; }  
    public decimal Amount { get; set; }
}