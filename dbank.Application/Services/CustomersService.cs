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
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DBank.Application.Services;

public class CustomersService(BankDbContext context, IRabbitMqProducer rabbitMqProducer, IEmailService emailService, 
                              IOptions<EmailOptions> emailOptions, IDistributedCache cache) : ICustomersService
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(emailOptions.Value.VerificationExpireMin)
    };
    
    public async Task<string> Create(CreateCustomerDto customer)
    {
        var userId = Guid.NewGuid().ToString();
        var verificationCode = emailService.GenerateCode();
        var email = customer.Email;
        var creationDate = DateTime.UtcNow;

        var verificationData = new VerificationData
        {
            VerificationCode = verificationCode,
            VerificationDate = creationDate,
            Customer = customer,
        };
        var serialisedData = JsonSerializer.Serialize(verificationData);
        
        await cache.SetStringAsync($"2fa-{userId}", serialisedData, _cacheOptions);
        await rabbitMqProducer.PrepareVerificationMessage(verificationCode, email);
        
        return userId;
    }

    public async Task<bool> Verification(VerificationDto verification)
    {
        var cachedData = await cache.GetStringAsync($"2fa-{verification.UserId}");
        if (string.IsNullOrEmpty(cachedData)) throw new EntityNotFoundException("Verification Code not found.");
        
        var verificationData = JsonConvert.DeserializeObject<dynamic>(cachedData);
        if (verificationData.VerificationCode != verification.VerificationCode)
            throw new Exception("Customer verification code does not match.");
        
        var timeExpired = DateTime.UtcNow - (DateTime)verificationData.CreationDate;
        if (timeExpired > TimeSpan.FromMinutes(emailOptions.Value.VerificationExpireMin)) 
            throw new Exception("Verification expired.");
        
        return true;
    }
    
    public async Task Save(VerificationDto verification)
    {
        var isVerified = await Verification(verification);

        if (isVerified)
        {
            var cachedData = await cache.GetStringAsync($"2fa-{verification.UserId}");
            if (string.IsNullOrEmpty(cachedData)) throw new Exception("Customer not verified.");
            var verificationData = JsonSerializer.Deserialize<VerificationData>(cachedData);
            var customer = verificationData!.Customer;

            var entity = new CustomerEntity
            {
                CustomerId = customer.CustomerId,
                Card = customer.Card,
                Phone = customer.Phone,
                MiddleName = customer.MiddleName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                BirthDate = customer.BirthDate
            };

            await context.Customers.AddAsync(entity);
            await context.SaveChangesAsync();

            await rabbitMqProducer.PrepareWelcomeMessage(entity);
            await cache.RemoveAsync($"2fa-{verification.UserId}");
        }
        else
        {
            throw new Exception("Customer not verified.");
        }
    }
    
    public async Task<CustomerDto> GetById(long customerId)
    {
        var entity = await context.Customers
            .Include(c => c.CashDeposits)
            .Include(b => b.Balance)
            .Include(t => t.Transactions)
            .Include(c => c.Credits)
            .FirstOrDefaultAsync(e => e.Id == customerId);

        if (entity == null)
        {
            throw new EntityNotFoundException($"Customer with id {customerId} not found.");
        }
        
        return entity.ToDto();
    }
}
