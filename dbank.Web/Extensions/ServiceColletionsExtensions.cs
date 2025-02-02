using System.Text;
using DBank.Application.Abstractions;
using DBank.Application.Services;
using DBank.Domain;
using DBank.Domain.Entities;
using DBank.Domain.Models;
using DBank.Domain.Options;
using DBank.Web.BackgroundServices;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
                    Title = "DBank API",
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
        
        public static WebApplicationBuilder AddHangfire(this WebApplicationBuilder builder)
        {
            builder.Services.AddHangfire(config => config
                .UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("Hangfire"))));
            
            builder.Services.AddHangfireServer();
            
            return builder;
        }
        
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICustomersService, CustomersService>();
            builder.Services.AddScoped<ITransactionsService, TransactionsService>();
            builder.Services.AddScoped<IBalancesService, BalancesService>();
            builder.Services.AddScoped<ICashDepositsService, CashDepositsService>();
            builder.Services.AddScoped<ICreditsService, CreditsService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            return builder;
        }

        public static WebApplicationBuilder AddIntegrationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICurrenciesImporter, CurrenciesImporter>();
            builder.Services.AddScoped<ICurrenciesService, CurrenciesService>();
            
            return builder;
        }

        public static WebApplicationBuilder AddBackgroundServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<CurrenciesRefreshSchedule>();
            builder.Services.AddHostedService<RabbitMqConsumer>();
            builder.Services.AddSingleton<RabbitMqProducer>();
            builder.Services.AddSingleton<IRabbitMqProducer>(provider => provider.GetRequiredService<RabbitMqProducer>());
            
            return builder;
        }

        public static WebApplicationBuilder AddBearerTokenAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(x => 
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => 
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.UseSecurityTokenValidators = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                        builder.Configuration["JWT:TokenPrivateKey"]!)),
                    ValidIssuer = builder.Configuration["JWT:TokenIssuer"],
                    ValidAudience = builder.Configuration["JWT:TokenAudience"],
                };
            });
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy => policy.RequireRole(RoleСonstants.Admin))
                .AddPolicy("User", policy => policy.RequireRole(RoleСonstants.User));

            builder.Services.AddDefaultIdentity<UserEntity>(options => 
            {
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<BankDbContext>()
            .AddUserManager<UserManager<UserEntity>>()
            .AddUserStore<UserStore<UserEntity, IdentityRoleEntity, BankDbContext, long>>();
            
            builder.Services.AddScoped<IIdentityService, IdentityService>();
            builder.Services.AddScoped<IJwtGenerateService, JwtGenerateService>();
            
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
            builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
            builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
            
            return builder;
        }
    }
}
