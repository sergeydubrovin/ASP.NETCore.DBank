using dbank.Application.Models.Payments;
using dbank.Domain.Entities;

namespace dbank.Application.Abstractions
{
    public interface IPaymentsService
    {
        Task Create(CreatePaymentDto payment);
        Task<PaymentEntity> GetById(long paymentId);
        Task<List<PaymentEntity>> GetByUser(long customerId);  
    }
}
