using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using dbank.Domain;

namespace dbank.Web.Extensions
{
    public static class ServiceColletionsExtensions
    {
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
            builder.Services.AddDbContext<BankDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Connect")));

            return builder;
        }
    
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            return builder;
        }
    
        public static WebApplicationBuilder AddIntеgrationServices(this WebApplicationBuilder builder)
        {
            return builder;
        }
    }
}   
