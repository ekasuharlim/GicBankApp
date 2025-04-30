namespace GicBankApp.Shared;

public class Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error FirstTransactionCannotBeWithdrawal =
        new("BANKACCOUNT.FIRST_TXN_WITHDRAWAL", "The first transaction cannot be a withdrawal.");

}