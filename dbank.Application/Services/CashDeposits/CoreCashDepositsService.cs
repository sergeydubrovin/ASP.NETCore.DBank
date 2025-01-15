using dbank.Application.Models.CashDeposits;

namespace dbank.Application.Services.CashDeposits;

public static class CoreCashDepositsService
{
    public static decimal ComputeFinalAmount(this CreateCashDepositDto deposit)
    {
        var monthlyInterestRate = deposit.InterestRate / 12m;

        var finalAmount = deposit.DepositAmount;

        for (decimal month = 0; month < deposit.DepositPeriod; month++)
        {
            finalAmount *= (1m + monthlyInterestRate);
        }
        
        return Math.Round(finalAmount);
    }
}
