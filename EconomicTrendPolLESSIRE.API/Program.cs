using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Infrastructure.Repositories;
using EconomicTrendsPolLESSIRE.Infrastructure.Services;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

// HTTP
builder.Services.AddHttpClient();

// SQL
var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

// Realtime publisher
builder.Services.AddSingleton<IMarketRealtimePublisher, SignalRMarketRealtimePublisher>();

//Services
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<IMarketCandleService, MarketCandleService>();
builder.Services.AddScoped<IMarketQuoteService, MarketQuoteService>();
builder.Services.AddScoped<IMarketSnapshotService, MarketSnapshotService>();
builder.Services.AddScoped<IMarketTradeService, MarketTradeService>();
builder.Services.AddScoped<IMessageCorrelationService, MessageCorrelationService>();
builder.Services.AddScoped<IMessageTriageService, MessageTriageService>();
// builder.Services.AddScoped<IProfanityService, ProfanityAdminService>();
builder.Services.AddScoped<ITechnicalIndicatorService, TechnicalIndicatorService>();
builder.Services.AddScoped<IUserHubService, UserHubService>();
builder.Services.AddScoped<IUserMessageService, UserMessageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserSessionsService, UserSessionsService>();



//Repositories
builder.Services.AddScoped<IInstrumentRepository, InstrumentRepository>();
builder.Services.AddScoped<IMarketCandleRepository, MarketCandleRepository>();
builder.Services.AddScoped<IMarketQuoteRepository, MarketQuoteRepository>();
builder.Services.AddScoped<IMarketSnapshotRepository, MarketSnapshotRepository>();
builder.Services.AddScoped<IMarketTradeRepository, MarketTradeRepository>();
builder.Services.AddScoped<IProviderInstrumentRepository, ProviderInstrumentRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<ITechnicalIndicatorRepository, TechnicalIndicatorRepository>();
builder.Services.AddScoped<IUserMessageAdminQueueRepository, UserMessageAdminQueueRepository>();
builder.Services.AddScoped<IUserMessageRepository, UserMessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSessionsRepository, UserSessionsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapHub<MarketDataHub>("/hubs/market-data");
app.MapHub<UserHub>($"/{UserHubMethods.HubPath}");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
