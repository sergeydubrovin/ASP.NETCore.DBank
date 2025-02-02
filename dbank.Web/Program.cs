using DBank.Web.Extensions;
using Hangfire;
using SwaggerThemes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder
    .AddBearerTokenAuthentication()
    .AddLogging()
    .AddSwagger()
    .AddData()
    .AddCache()
    .AddHangfire()
    .AddApplicationServices()
    .AddIntegrationServices()
    .AddBackgroundServices()
    .AddHttpClients()
    .AddOptions();

var app = builder.Build();

app.UseHttpLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(Theme.Monokai);
app.UseHangfireDashboard();
app.MapControllers();

app.Run();
