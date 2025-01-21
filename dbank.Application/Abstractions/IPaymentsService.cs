using DBank.Application.Models.Payments;
using DBank.Domain.Entities;

namespace DBank.Application.Abstractions
{
    public interface IPaymentsService
    {
        Task Create(CreatePaymentDto payment);
        Task<PaymentEntity> GetById(long paymentId);
        Task<List<PaymentEntity>> GetByUser(long customerId);  
        Task<List<PaymentEntity>> GetAll();
    }
}
