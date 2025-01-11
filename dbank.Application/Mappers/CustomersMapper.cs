using dbank.Application.Models.Balances;
using dbank.Application.Models.Customers;
using dbank.Domain.Entities;

namespace dbank.Application.Mappers;

public static class CustomersMapper
{
    public static CustomerDto ToDto(this CustomerEntity entity, BalanceEntity? balance = null)
    {
        return new CustomerDto
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId!.Value,
            CardNumber = entity.CardNumber,
            Phone = entity.Phone,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            MiddleName = entity.MiddleName,
            BirthDate = entity.BirthDate,
            Balance = balance == null ? entity.Balance?.ToDto().Balance : balance.ToDto().Balance
        };
    }
    
    public static CustomerEntity ToEntity(this CreateCustomerDto entity, BalanceDto? balance = null)
    {
        return new CustomerEntity
        {
            CustomerId = entity.CustomerId,
            CardNumber = entity.CardNumber,
            Phone = entity.Phone,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            MiddleName = entity.MiddleName,
            BirthDate = entity.BirthDate,
            Balance = balance?.ToEntity()
        };
    }
}
