using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using EconomicTrendsPolLESSIRE.Infrastructure.Repositories;
using EconomicTrendsPolLESSIRE.Infrastructure.Security;
using EconomicTrendsPolLESSIRE.Infrastructure.Services;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers / API
builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

// HTTP
builder.Services.AddHttpClient();

// SQL
var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException( "Connection string 'Default' not found.");

builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

// Realtime publisher
builder.Services.AddSingleton<IMarketRealtimePublisher, SignalRMarketRealtimePublisher>();

//Services
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<ILocalAiContextService, LocalAiContextService>();
builder.Services.AddScoped<IMarketCandleService, MarketCandleService>();
builder.Services.AddScoped<IMarketQuoteService, MarketQuoteService>();
builder.Services.AddScoped<IMarketSnapshotService, MarketSnapshotService>();
builder.Services.AddScoped<IMarketTradeService, MarketTradeService>();
builder.Services.AddScoped<IMessageCorrelationService, MessageCorrelationService>();
builder.Services.AddScoped<IMessageTriageService, MessageTriageService>();
builder.Services.AddScoped<IProfanityService, ProfanityService>();
builder.Services.AddScoped<IProfanityAdminService, ProfanityAdminService>();
builder.Services.AddScoped<ITechnicalIndicatorService, TechnicalIndicatorService>();
builder.Services.AddScoped<IUserHubService, UserHubService>();
builder.Services.AddScoped<IUserMessageService, UserMessageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserSessionsService, UserSessionsService>();



//Repositories
builder.Services.AddScoped<IInstrumentRepository, InstrumentRepository>();
builder.Services.AddScoped<ILocalAiDataRepository, LocalAiDataRepository>();
builder.Services.AddScoped<IMarketCandleRepository, MarketCandleRepository>();
builder.Services.AddScoped<IMarketQuoteRepository, MarketQuoteRepository>();
builder.Services.AddScoped<IMarketSnapshotRepository, MarketSnapshotRepository>();
builder.Services.AddScoped<IMarketTradeRepository, MarketTradeRepository>();
builder.Services.AddScoped<IProviderInstrumentRepository, ProviderInstrumentRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProfanityRepository, ProfanityRepository>();
builder.Services.AddScoped<ITechnicalIndicatorRepository, TechnicalIndicatorRepository>();
builder.Services.AddScoped<IUserMessageAdminQueueRepository, UserMessageAdminQueueRepository>();
builder.Services.AddScoped<IUserMessageRepository, UserMessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSessionsRepository, UserSessionRepository>();

builder.Services.AddScoped<IPasswordHasher<Users>, Argon2PasswordHasher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapHub<MarketDataHub>("/hubs/market-data");
app.MapHub<UserHub>($"/{UserHubMethods.HubPath}");

app.Run();



































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.