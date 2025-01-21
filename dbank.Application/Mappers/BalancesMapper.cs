using DBank.Application.Models.Balances;
using DBank.Domain.Entities;

namespace DBank.Application.Mappers;

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
