using DBank.Application.Models.Balances;
using DBank.Application.Models.Cards;
using DBank.Application.Models.Customers;
using DBank.Domain.Entities;

namespace DBank.Application.Mappers;

public static class CustomersMapper
{
    public static CustomerDto ToDto(this CustomerEntity entity, CardEntity? card = null, BalanceEntity? balance = null)
    {
        return new CustomerDto
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId!.Value,
            Card = card == null ? entity.Card!.ToDto().Card : card.ToDto().Card,
            Phone = entity.Phone,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            MiddleName = entity.MiddleName,
            BirthDate = entity.BirthDate,
            Balance = balance == null ? entity.Balance?.ToDto().Balance : balance.ToDto().Balance,
            CashDepositsCount = entity.CashDeposits!.Count,
            TransactionsCount = entity.Transactions!.Count,
            CreditsCount = entity.Credits!.Count,
        };
    }
    
    public static CustomerEntity ToEntity(this CreateCustomerDto entity, CardDto? card = null, BalanceDto? balance = null)
    {
        return new CustomerEntity
        {
            CustomerId = entity.CustomerId,
            Card = card?.ToEntity(),
            Phone = entity.Phone,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            MiddleName = entity.MiddleName,
            Email = entity.Email,
            BirthDate = entity.BirthDate,
            Balance = balance?.ToEntity(),
        };
    }
}
