using DBank.Application.Abstractions;
using DBank.Application.Services;
using DBank.Domain;
using DBank.Domain.Options;
using DBank.Web.BackgroundServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpLogging;

namespace DBank.Web.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpLogging(option =>
                option.LoggingFields = HttpLoggingFields.Duration);

            return builder;
        }

        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1"
                });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return builder;
        }

        public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<BankDbContext>(option =>
                option.UseNpgsql(builder.Configuration.GetConnectionString("Db")));

            return builder;
        }

        public static WebApplicationBuilder AddCache(this WebApplicationBuilder builder)
        {
            builder.Services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = builder.Configuration.GetConnectionString("Redis");
            });
            
            return builder;
        }
        
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICustomersService, CustomersService>();
            builder.Services.AddScoped<IPaymentsService, PaymentsService>();
            builder.Services.AddScoped<IBalancesService, BalancesService>();
            builder.Services.AddScoped<ICashDepositsService, CashDepositsService>();
            builder.Services.AddScoped<ICreditsService, CreditsService>();

            return builder;
        }

        public static WebApplicationBuilder AddIntegrationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICurrenciesImporter, CurrenciesImporter>();
            builder.Services.AddScoped<ICurrenciesService, CurrenciesService>();
            
            return builder;
        }

        public static WebApplicationBuilder AddHostedServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<CurrenciesBackground>();
            
            return builder;
        }
        
        public static WebApplicationBuilder AddHttpClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient("Cb", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Integrations:CbImporter:BaseUrl"]!);
            });
            
            return builder;
        }
        
        public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<CbOptions>(builder.Configuration.GetSection("Integrations:CbImporter"));
            
            return builder;
        }
    }
}
