using DBank.Application.Models.CashDeposits;
using DBank.Application.Services;
using DBank.Domain;
using DBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTestApplication.ApplicationTests;

public class CashDepositsTest
{
    [Fact]
    private async Task Create_ValidCashDeposit_ComputeFinalAmount_AccruedInterest()
    {
        // Arrange
        var cashDepositDto = new CreateCashDepositDto
        {
            CustomerId = 1,
            DepositAmount = 100000,
            DepositPeriod = 8,
            InterestRate = 0.21m
        };

        const decimal finalAmount = 114888.18m;
        const decimal accruedInterest = 14888.18m;
        
        var options = new DbContextOptionsBuilder<BankDbContext>().Options;
        var mockContext = new Mock<BankDbContext>(options);
        var mockCashDepDbSet = new Mock<DbSet<CashDepositEntity>>();
        
        mockContext.Setup(m => m.CashDeposits).Returns(mockCashDepDbSet.Object);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        var cashDepService = new CashDepositsService(mockContext.Object);
        
        // Act
        var result = await cashDepService.Create(cashDepositDto);
        
        // Asserts
        Assert.Equal(finalAmount, result.FinalAmount);
        Assert.Equal(accruedInterest, result.AccruedInterest);
    }
}
