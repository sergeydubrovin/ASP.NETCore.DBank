using DBank.Application.Models.Balances;
using DBank.Domain.Entities;

namespace DBank.Application.Abstractions;

public interface IBalancesService
{
    Task Create(CreateBalanceDto balance);
    Task<BalanceEntity> GetByUser(long customerId);
}
