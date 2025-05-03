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

    public static Error BankAccountNotFound =
        new("BANKACCOUNT.NOT_FOUND", "Account number not found.");

    public static Error FirstTransactionCannotBeWithdrawal =
        new("BANKACCOUNT.FIRST_TXN_WITHDRAWAL", "The first transaction cannot be a withdrawal.");
        
    public static Error InsufficentBalance =
        new("BANKACCOUNT.INSUFFICENT_BALANCE", "Insufficent Balance.");

    public static Error TransactionDateMustBeAfterLastTransaction =
        new("BANKACCOUNT.TXN_DATE_MUST_BE_AFTER_LAST_TXN", "Transaction date must be the same or after the last transaction date.");

    public static Error InvalidInterestRateAmount =
        new("INTERESTRULE.INVALID_RATE_AMOUNT", "Interest rate should be greater than 0 and less than 100");

    public static Error InvalidMonthlyPeriodMonth =
        new("MONTHLYPERIOD.INVALID_MONTH", "Monthly Period Month must be between 1 and 12");


}