using dbank.Application.Models.Balances;
using dbank.Domain.Entities;

namespace dbank.Application.Mappers;

public static class BalancesMapper
{
    public static BalanceDto ToDto(this BalanceEntity entity)
    {
        return new BalanceDto
        {
            CustomerId = entity.CustomerId,
            Balance = entity.Balance
        };
    }
    
    public static BalanceEntity ToEntity(this BalanceDto dto)
    {
        return new BalanceEntity
        {
            Balance = dto.Balance
        };
    }
}
