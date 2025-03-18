using DBank.Application.Models.CashDeposits;
using DBank.Application.Models.Credits;
using DBank.Application.Models.Transactions;
using DBank.Domain.Entities;
using DBank.Domain.Exceptions;

namespace DBank.Application.Extensions;

public static class ValidationExtensions
{
    public static void ValidationTransaction(this CreateTransactionsDto? payment, CustomerEntity? sender,
                                             CustomerEntity? recipient)
    {
        switch (sender, recipient)
        {
            case (null, _):
                throw new EntityNotFoundException($"Sender with id {payment!.CustomerId} not found.");

            case (_, null):
                throw new EntityNotFoundException(
                    $"The recipient with card number {payment!.RecipientCard} not found.");

            case ({ Balance: null }, _):
                throw new EntityNotFoundException($"The sender with id {payment!.CustomerId} doesn't have open balance.");

            case ({ Balance: { Balance: var senderBalance } }, _)
                when senderBalance < payment!.TransactionAmount:
                throw new ArgumentException("There are insufficient funds on balance.");
        }
    }

    public static void ValidationCashDeposit(this CreateCashDepositDto depositDto)
    {
        if(depositDto.DepositAmount < 100000) 
            throw new ArgumentException("Deposit amount cannot be less than 100.000");
        
        if(depositDto.DepositPeriod is < 3 or > 36) 
            throw new ArgumentException("Deposit period must be between 3 and 36");
        
        if(depositDto.InterestRate != 0.21m) 
            throw new ArgumentException("The interest rate is 21%, it is constant");
    }

    public static void ValidationCredit(this CreateCreditDto creditDto)
    {
        if(creditDto.CreditAmount is < 1000000 or 10000000)
            throw new ArgumentException("Credit amount must be between 1.000.000 and 10.000.000");
        
        if(creditDto.CreditPeriod is < 1 or > 10)
            throw new ArgumentException("Credit period must be between 1 and 10");
        
        if(creditDto.InterestRate != 0.31m)
            throw new ArgumentException("The interest rate is 31%, it is constant");
        
        if(creditDto.InitialPaymentRate != 0.28m)
            throw new ArgumentException("Initial payment rate is 28%, it is constant");
    }
}
