using dbank.Application.Abstractions;
using dbank.Application.Models.Payments;
using dbank.Domain;
using dbank.Domain.Entities;
using dbank.Domain.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace dbank.Application.Services.Payments;

public class PaymentsService(BankDbContext context) : IPaymentsService
{
    public async Task Create(CreatePaymentDto payment)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var sender = await context.Customers
                .Include(b => b.Balance)
                .FirstOrDefaultAsync(s => s.CustomerId == payment.CustomerId);
            var recipient = await context.Customers
                .Include(b => b.Balance)
                .FirstOrDefaultAsync(r => r.CardNumber == payment.RecipientCardNumber);

            payment.ValidationPayment(sender, recipient);

            sender!.Balance!.Balance -= payment.PaymentAmount;

            if (recipient!.Balance == null)
            {
                recipient.Balance = new BalanceEntity()
                {
                    CustomerId = recipient.CustomerId,
                    Balance = payment.PaymentAmount,
                };
                context.Balances.Add(recipient.Balance);
            }
            else
            {
                recipient.Balance.Balance += payment.PaymentAmount;
            }

            var entity = new PaymentEntity()
            {
                PaymentAmount = payment.PaymentAmount,
                Name = payment.Name,
                RecipientCardNumber = payment.RecipientCardNumber,
                CustomerId = payment.CustomerId,
            };

            await context.Payments.AddAsync(entity);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (EntityNotFoundException ex)
        {
            await transaction.RollbackAsync();

            throw new PaymentErrorException($"Ошибка при обработке платежа. {ex.Message}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            throw new PaymentErrorException($"Непредвиденная ошибка при обработке платежа. {ex.Message}");
        }
    }
    
    public async Task<PaymentEntity> GetById(long paymentId)
    {
        var entity = await context.Payments.FirstOrDefaultAsync(e => e.Id == paymentId);

        if (entity == null)
        {
            throw new EntityNotFoundException($"Платеж по id {paymentId} не найден.");
        }

        return entity;
    }
    
    public async Task<List<PaymentEntity>> GetByUser(long customerId)
    {
        var payments = await context.Payments.Where(p => p.CustomerId == customerId).ToListAsync();

        return payments;
    }
    
    public async Task<List<PaymentEntity>> GetAll()
    {
        var payments = await context.Payments.ToListAsync();
            
        return payments;
    }
}
