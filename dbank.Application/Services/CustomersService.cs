using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Mappers;
using DBank.Application.Models.Customers;
using DBank.Domain;
using DBank.Domain.Entities;
using DBank.Domain.Exceptions;
using DBank.Domain.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DBank.Application.Services;

public class CustomersService(BankDbContext context, IRabbitMqService rabbitMqService, IEmailService emailService, 
                              IOptions<EmailOptions> emailOptions, IDistributedCache cache) : ICustomersService
{
    private readonly EmailOptions _emailOptions = emailOptions.Value;
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(emailOptions.Value.VerificationExpireMin)
    };
    
    public async Task<long> Create(CreateCustomerDto customer)
    {
        var entity = new CustomerEntity
        {
            CustomerId = customer.CustomerId,
            Phone = customer.Phone,
            MiddleName = customer.MiddleName,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            BirthDate = customer.BirthDate,
            IsVerified = false,
        };
        
        await context.Customers.AddAsync(entity);
        await context.SaveChangesAsync();
        
        var verificationCode = emailService.GenerateCode();
        var verificationData = new VerificationData
        {
            VerificationCode = verificationCode,
            VerificationDate = DateTime.UtcNow,
            CustomerId = customer.CustomerId,
        };
        var serialisedData = JsonSerializer.Serialize(verificationData);
        
        await cache.SetStringAsync($"2fa-{customer.CustomerId}", serialisedData, _cacheOptions);
        await rabbitMqService.PrepareVerificationMessage(verificationCode, entity.Email);
        
        return customer.CustomerId;
    }

    public async Task ValidateCode(VerificationDto verification)
    {
        var cachedData = await cache.GetStringAsync($"2fa-{verification.CustomerId}");
        if (string.IsNullOrEmpty(cachedData)) throw new EntityNotFoundException("Verification Code not found.");
        
        var verificationData = JsonSerializer.Deserialize<VerificationData>(cachedData);
        if (verificationData!.VerificationCode != verification.VerificationCode)
            throw new Exception("Customer verification code does not match.");
        
        var timeExpired = DateTime.UtcNow - verificationData.VerificationDate;
        if (timeExpired > TimeSpan.FromMinutes(_emailOptions.VerificationExpireMin)) 
            throw new Exception("Verification expired.");
    }
    
    public async Task CompleteVerification(VerificationDto verification)
    {
        await ValidateCode(verification);
        
        var cachedData = await cache.GetStringAsync($"2fa-{verification.CustomerId}");
        if (string.IsNullOrEmpty(cachedData)) throw new Exception("Verification Code not found.");
        
        var verificationData = JsonSerializer.Deserialize<VerificationData>(cachedData);
        var customerId = verificationData!.CustomerId;
            
        var entity = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        if (entity == null) throw new EntityNotFoundException($"Customer with id {verification.CustomerId} not found.");
        entity.IsVerified = true;
        
        await context.SaveChangesAsync();
        await rabbitMqService.PrepareWelcomeMessage(entity);
        await cache.RemoveAsync($"2fa-{verification.CustomerId}");
    }

    public async Task ResendCode(long customerId)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        
        if(customer == null) throw new EntityNotFoundException($"Customer with id {customerId} not found.");
        if (customer.IsVerified)
        {
            throw new Exception("Customer is already verified.");
        }
        
        var verificationCode = emailService.GenerateCode();
        var verificationData = new VerificationData
        {
            VerificationCode = verificationCode,
            VerificationDate = DateTime.UtcNow,
            CustomerId = customerId
        };
        
        var serialisedData = JsonSerializer.Serialize(verificationData);
        
        await cache.SetStringAsync($"2fa-{customerId}", serialisedData, _cacheOptions);
        await rabbitMqService.PrepareVerificationMessage(verificationCode, customer.Email);
    }
    
    public async Task<CustomerDto> GetById(long customerId)
    {
        var entity = await context.Customers
            .Include(c => c.Card)
            .Include(c => c.CashDeposits)
            .Include(b => b.Balance)
            .Include(t => t.Transactions)
            .Include(c => c.Credits)
            .FirstOrDefaultAsync(e => e.Id == customerId);

        if (entity == null) throw new EntityNotFoundException($"Customer with id {customerId} not found.");
        
        return entity.ToDto();
    }
}
