using DBank.Application.Abstractions;
using DBank.Application.Models.RabbitMq;
using DBank.Application.Services;
using DBank.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UnitTestApplication.ApplicationTestsExtensions;

namespace UnitTestApplication.ApplicationTests;

public class TransactionsTests
{
    // Arrange
    [Fact]
    private async Task Create_Valid_Transaction()
    {
        var (sender, recipient, transactionDto) = Extensions.CreateTransactionData();
        
        var options = new DbContextOptionsBuilder<BankDbContext>()
            .UseInMemoryDatabase(databaseName: "TransactionsTest")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;

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
