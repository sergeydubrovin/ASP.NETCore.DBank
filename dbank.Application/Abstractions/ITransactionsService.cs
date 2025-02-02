using DBank.Application.Models.Transactions;
using DBank.Domain.Entities;

namespace DBank.Application.Abstractions
{
    public interface ITransactionsService
    {
        Task Create(CreateTransactionsDto transactions);
        Task<TransactionEntity> GetById(long paymentId);
        Task<List<TransactionEntity>> GetByUser(long customerId);  
        Task<List<TransactionEntity>> GetAll();
    }
}
