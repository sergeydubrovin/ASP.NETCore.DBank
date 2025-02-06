using DBank.Application.Abstractions;
using DBank.Application.Extensions;
using DBank.Application.Models.RabbitMq;
using DBank.Application.Models.Transactions;
using DBank.Domain;
using DBank.Domain.Entities;
using DBank.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DBank.Application.Services;

public class TransactionsService(BankDbContext context, IRabbitMqService rabbitMqService) : ITransactionsService
{
    public async Task Create(CreateTransactionsDto transactions)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var sender = await context.Customers
                .Include(b => b.Balance)
                .FirstOrDefaultAsync(s => s.CustomerId == transactions.CustomerId);
            var recipient = await context.Customers
                .Include(b => b.Balance)
                .Include(c => c.Card)
                .FirstOrDefaultAsync(r => r.Card!.Card == transactions.RecipientCard);

            transactions.ValidationTransaction(sender, recipient);

            sender!.Balance!.Balance -= transactions.TransactionAmount;

            if (recipient!.Balance == null)
            {
                recipient.Balance = new BalanceEntity
                {
                    CustomerId = recipient.CustomerId,
                    Balance = transactions.TransactionAmount,
                };
                context.Balances.Add(recipient.Balance);
            }
            else
            {
                recipient.Balance.Balance += transactions.TransactionAmount;
            }

            var entity = new TransactionEntity
            {
                TransactionAmount = transactions.TransactionAmount,
                Name = transactions.Name!,
                RecipientCard = transactions.RecipientCard,
                CustomerId = transactions.CustomerId,
            };

            await context.Transactions.AddAsync(entity);
            await context.SaveChangesAsync();
            
            var message = new CreateTransactionMessage
            {
                SenderFirstName = sender.FirstName,
                SenderMiddleName = sender.MiddleName,
                RecipientCard = recipient.Card!.Card,
                RecipientEmail = recipient.Email,
                TransactionAmount = transactions.TransactionAmount
            };
            await rabbitMqService.PrepareTransactionMessage(message);
            
            await transaction.CommitAsync();
        }
        catch (EntityNotFoundException ex)
        {
            await transaction.RollbackAsync();

            throw new Exception($"Payment processing error. Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            throw new Exception($"An unexpected error occurred while processing the transactions. Error: {ex.Message}");
        }
    }
    
    public async Task<TransactionEntity> GetById(long paymentId)
    {
        var entity = await context.Transactions.FirstOrDefaultAsync(e => e.Id == paymentId);

        if (entity == null)
        {
            throw new EntityNotFoundException($"Payment with id {paymentId} not found.");
        }

        return entity;
    }
    
    public async Task<List<TransactionEntity>> GetByUser(long customerId)
    {
        var payments = await context.Transactions.Where(p => p.CustomerId == customerId).ToListAsync();

        return payments;
    }
    
    public async Task<List<TransactionEntity>> GetAll()
    {
        var payments = await context.Transactions.ToListAsync();
            
        return payments;
    }
}
