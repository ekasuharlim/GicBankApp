# GicBankApp

Assumptions:
User can't enter back dated transaction


Print statement: month 202306

previousMonthBalance = Sum all the previous transaction before first day of the period

Get EOD Balance:

EodBalance
    StartDate
    EndDate
    Balance


currentEodBalance
    StartDate = first Date 
    EndDate = empty
    Balance = previousMonthBalance

for each date in the month
    if date in transaction list:
        dateBalance = sum all transaction balance in that date
        if date = currentEodBalance.StartDate
            currentEodBalance.Balance += dateBalance
        else 
            currentEodBalance.EndDate = date - 1
            push currentEodBalance to List Of EodBalance
            previousEodBalance = currentEodBalance
            create new currentEodBalance
                StartDate = date
                EndDate = empty
                balance =  previouseEodBalance + dateBalance
                
currentEodBalance.EndDate = lastDate of month
push currentEodBalance to list of EodBalance
