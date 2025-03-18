using DBank.Application.Models.Credits;
using DBank.Application.Services;
using DBank.Domain;
using DBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTestApplication.ApplicationTests;

public class CreditsTests
{
    [Fact]
    private async Task Create_ValidCredit_ComputeInitialPayment_MonthlyPayment()
    {
        // Arrange
        var creditDto = new CreateCreditDto
        {
            CustomerId = 1,
            CreditAmount = 1000000,
            CreditPeriod = 3,
            InterestRate = 0.31m,
            InitialPaymentRate = 0.28m
        };

        const decimal expectedInitialPayment = 280000;
        const decimal expectedMonthlyPayment = 30960.83m;
        
        var options = new DbContextOptionsBuilder<BankDbContext>().Options;
        var mockContext = new Mock<BankDbContext>(options);
        var mockCreditDbSet = new Mock<DbSet<CreditEntity>>();
        
        mockContext.Setup(m => m.Credits).Returns(mockCreditDbSet.Object);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        var creditService = new CreditsService(mockContext.Object);

        // Act
        var result = await creditService.Create(creditDto);

        // Asserts
        Assert.Equal(expectedInitialPayment, result.InitialPayment);
        Assert.Equal(expectedMonthlyPayment, result.MonthlyPayment);
    }

    [Fact]
    private async Task Create_InvalidCreditDto_ThrowsArgumentException()
    {
        // Arrange
        var invalidDto = new CreateCreditDto
        {
            CustomerId = 1,
            CreditAmount = 8000,
            CreditPeriod = 11,
            InterestRate = 0.11m,
            InitialPaymentRate = 0.01m
        };
        
        var options = new DbContextOptionsBuilder<BankDbContext>().Options;
        var mockContext = new Mock<BankDbContext>(options);
        var mockCreditDbSet = new Mock<DbSet<CreditEntity>>();
        
        mockContext.Setup(m => m.Credits).Returns(mockCreditDbSet.Object);
        
        var creditService = new CreditsService(mockContext.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => creditService.Create(invalidDto));
    }
}
