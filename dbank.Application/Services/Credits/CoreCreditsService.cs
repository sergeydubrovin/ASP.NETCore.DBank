using dbank.Application.Models.Credits;

namespace dbank.Application.Services.Credits;

public static class CoreCreditsService
{
    public static decimal ComputeInitialPayment(this CreateCreditDto credit)
    {
        var initialPayment = credit.CreditAmount * credit.InitialPaymentRate;
        
        return initialPayment;
    }

    public static decimal ComputeMonthlyPayment(this CreateCreditDto credit, decimal initialPayment)
    {
        var residualCreditAmount = credit.CreditAmount - initialPayment;
        var monthlyInterestRate = credit.InterestRate / 12;
        var numberOfMonths = credit.CreditPeriod * 12;

        var interestPowerTerm = 1m;

        for (var i = 0; i < numberOfMonths; i++)
        {
            interestPowerTerm *= (1 + monthlyInterestRate);
        }
        
        var numerator = residualCreditAmount * monthlyInterestRate * interestPowerTerm;
        var denominator = interestPowerTerm - 1;
        
        var monthlyPayment = numerator / denominator;
        
        return Math.Round(monthlyPayment);
    }
}
