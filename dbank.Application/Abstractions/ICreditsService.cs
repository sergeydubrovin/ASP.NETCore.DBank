using dbank.Application.Models.Credits;
using dbank.Domain.Entities;

namespace dbank.Application.Abstractions;

public interface ICreditsService
{
    Task Create(CreateCreditDto credit);
    Task<CreditEntity> GetById(long creditId);
    Task<List<CreditEntity>> GetByUser(long customerId);
}
