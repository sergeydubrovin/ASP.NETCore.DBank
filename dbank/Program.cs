using dbank.Web.Extensions;
using Microsoft.AspNetCore.HttpLogging;
using SwaggerThemes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpLogging(opt =>
{
    opt.LoggingFields = HttpLoggingFields.Duration;
});

builder
    .AddSwagger()
    .AddData()
    .AddApplicationServices()
    .AddIntegrationServices();

var app = builder.Build();

app.UseHttpLogging();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(Theme.Monokai);
app.MapControllers();

app.Run();
