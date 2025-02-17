using DBank.Application.Abstractions;
using DBank.Application.Models.RabbitMq;
using DBank.Application.Models.Transactions;
using DBank.Application.Services;
using DBank.Domain;
using DBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTestApplication.ApplicationTests;

public class TransactionsTest
{
    // Arrange
    [Fact]
    private async Task Create_Valid_Transaction()
    {
        var sender = new CustomerEntity
        {
            CustomerId = 1,
            FirstName = "sender",
            MiddleName = "sender",
            LastName = "sender",
            Email = "sender.@gmail.com",
            Phone = "89511103388",
            Card = new CardEntity { CustomerId = 1, Card = "1234123412341234" },
            Balance = new BalanceEntity { CustomerId = 1, Balance = 1000m }
        };
        var recipient = new CustomerEntity
        {
            CustomerId = 2,
            FirstName = "recipient",
            MiddleName = "recipient",
            LastName = "recipient",
            Email = "recipient.@gmail.com",
            Phone = "89511103388",
            Card = new CardEntity { CustomerId = 2, Card = "1234123412344321" },
            Balance = new BalanceEntity { CustomerId = 2, Balance = 1000m }
        };
        var transactionDto = new CreateTransactionsDto
        {
            CustomerId = sender.CustomerId,
            RecipientCard = recipient.Card.Card,
            TransactionAmount = 300m,
            Name = "testTransaction"
        };
        
        var options = new DbContextOptionsBuilder<BankDbContext>()
            .UseInMemoryDatabase(databaseName: "TransactionsTest")
            .ConfigureWarnings(w => 
                w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var mockRabbit = new Mock<IRabbitMqService>();
        mockRabbit.Setup(x => x.PrepareTransactionMessage(It.IsAny<CreateTransactionMessage>()))
            .Returns(Task.CompletedTask);
        
        await using (var context = new BankDbContext(options))
        {
            context.Customers.AddRange(sender, recipient);
            await context.SaveChangesAsync();
            
            var transactionService = new TransactionsService(context, mockRabbit.Object);

            // Act
            await transactionService.Create(transactionDto);
        }
        
        // Assert
        await using (var assertContext = new BankDbContext(options))
        {
             var refreshedSender = await assertContext.Customers
                 .Include(x => x.Balance)
                 .FirstOrDefaultAsync(c => c.CustomerId == sender.CustomerId);
             
             var refreshedRecipient = await assertContext.Customers
                 .Include(x => x.Balance)
                 .FirstOrDefaultAsync(c => c.CustomerId == recipient.CustomerId);
             
             Assert.NotNull(refreshedSender);
             Assert.NotNull(refreshedRecipient);
             
             Assert.Equal(700, refreshedSender.Balance!.Balance);
             Assert.Equal(1300, refreshedRecipient.Balance!.Balance);
        }
    }
}
