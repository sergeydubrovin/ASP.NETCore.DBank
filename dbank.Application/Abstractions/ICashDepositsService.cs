using dbank.Application.Models.CashDeposits;
using dbank.Domain.Entities;

namespace dbank.Application.Abstractions;

public interface ICashDepositsService
{
    Task Create(CreateCashDepositDto dto);
    Task<CashDepositEntity> GetById(long depositId);
    Task<List<CashDepositEntity>> GetByUser(long customerId);
}
