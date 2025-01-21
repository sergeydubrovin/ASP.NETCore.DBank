using DBank.Application.Models.CashDeposits;
using DBank.Domain.Entities;

namespace DBank.Application.Abstractions;

public interface ICashDepositsService
{
    Task<CashDepositEntity> Create(CreateCashDepositDto dto);
    Task<CashDepositEntity> GetById(long depositId);
    Task<List<CashDepositEntity>> GetByUser(long customerId);
    Task<List<CashDepositEntity>> GetAll();
}
