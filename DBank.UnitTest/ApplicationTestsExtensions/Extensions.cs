using System.Net;
using System.Text;
using DBank.Application.Models.Currencies;
using DBank.Application.Models.Transactions;
using DBank.Domain.Entities;
using Moq;
using Moq.Protected;

namespace UnitTestApplication.ApplicationTestsExtensions;

public static class Extensions
{
    public static ValuteDto CreateExpectedValuteDto()
    {
        return new ValuteDto
        {
            JPY = new ValuteItemDto
            {
                Id = "R01820",
                NumCode = "392",
                CharCode = "JPY",
                Nominal = 100,
                Name = "Иен",
                Value = 60.2019m,
                Previous = 59.1033m
            },
            USD = new ValuteItemDto { Id = "" },
            EUR = new ValuteItemDto { Id = "" },
            AED = new ValuteItemDto { Id = "" },
            CNY = new ValuteItemDto { Id = "" },
            KRW = new ValuteItemDto { Id = "" },
            THB = new ValuteItemDto { Id = "" },
            CAD = new ValuteItemDto { Id = "" }
        };
    }

    public static Mock<HttpMessageHandler> GetMockHandler(this Mock<HttpMessageHandler> mockHandler,
                                                          HttpStatusCode statusCode, string content)
    {
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content, Encoding.UTF8)
            })
            .Verifiable();
        return mockHandler;
    }

    public static (CustomerEntity sender, CustomerEntity recipient, 
                   CreateTransactionsDto transactionDto) CreateTransactionData()
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
        return (sender, recipient, transactionDto);
    }
}
