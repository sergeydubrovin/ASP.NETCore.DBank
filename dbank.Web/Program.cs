using DBank.Web.Extensions;
using SwaggerThemes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder
    .AddLogging()
    .AddSwagger()
    .AddData()
    .AddCache()
    .AddApplicationServices()
    .AddIntegrationServices()
    .AddHostedServices()
    .AddHttpClients()
    .AddOptions();

var app = builder.Build();

app.UseHttpLogging();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(Theme.Monokai);
app.MapControllers();

app.Run();
