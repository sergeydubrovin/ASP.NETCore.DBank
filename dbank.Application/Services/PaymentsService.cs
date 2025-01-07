using dbank.Application.Abstractions;
using dbank.Application.Models.Payments;
using dbank.Domain;
using dbank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dbank.Application.Services
{
    public class PaymentsService(BankDbContext context) : IPaymentsService
    {
        public async Task Create(CreatePaymentDto pay)
        {
            var entity = new PaymentEntity
            {
                PaymentAmount = pay.PaymentAmount,
                Name = pay.Name,
                RecipientCardNumber = pay.RecipientCardNumber,
                CustomerId = pay.CustomerId
            };       
            await context.Payments.AddAsync(entity);
            await context.SaveChangesAsync();
        }
        public async Task<PaymentEntity> GetById(long paymentId)
        {
            var entity = await context.Payments.FirstOrDefaultAsync(e => e.Id == paymentId);

            return entity;
        }
        public async Task<List<PaymentEntity>> GetByUser(long customerId)
        {
            var payments = await context.Payments.Where(p => p.CustomerId == customerId).ToListAsync();

            return payments;
        }
    }
}

