using DBank.Application.Models.Credits;
using DBank.Domain.Entities;

namespace DBank.Application.Abstractions;

public interface ICreditsService
{
    Task<CreditEntity> Create(CreateCreditDto credit);
    Task<CreditEntity> GetById(long creditId);
    Task<List<CreditEntity>> GetByUser(long customerId);
    Task<List<CreditEntity>> GetAll();
}
