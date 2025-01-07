using dbank.Application.Models.Balances;
using dbank.Domain.Entities;

namespace dbank.Application.Abstractions;

public interface IBalancesService
{
    Task Create(CreateBalanceDto balance);
    Task<BalanceEntity> GetByUser(long customerId);
}
