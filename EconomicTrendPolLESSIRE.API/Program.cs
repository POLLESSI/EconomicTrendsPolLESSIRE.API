using EconomicTrendsPolLESSIRE.API.BackgroundServices;
using EconomicTrendsPolLESSIRE.API.Options;
using EconomicTrendsPolLESSIRE.API.Tools;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Application.MarketData;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using EconomicTrendsPolLESSIRE.Infrastructure.MarketData;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Repositories;
using EconomicTrendsPolLESSIRE.Infrastructure.Repositories;
using EconomicTrendsPolLESSIRE.Infrastructure.Security;
using EconomicTrendsPolLESSIRE.Infrastructure.Services;
using EconomicTrendsPolLESSIRE.Shared.StaticConfig.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Controllers / API
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter the JWT access token."
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

// SignalR
builder.Services.AddSignalR();

// HTTP
builder.Services.AddHttpClient();

// SQL
var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException( "Connection string 'Default' not found.");

builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

// Realtime publisher

var mongoConnectionString = builder.Configuration["Mongo:ConnectionString"] ?? throw new InvalidOperationException("Mongo:ConnectionString is missing.");

var mongoDatabaseName = builder.Configuration["Mongo:DatabaseName"] ?? throw new InvalidOperationException("Mongo:DatabaseName is missing.");

builder.Services.AddSingleton<IMongoClient>( _ => new MongoClient(mongoConnectionString));

builder.Services.AddSingleton<IMongoDatabase>(
    sp =>
        sp.GetRequiredService<IMongoClient>()
          .GetDatabase(mongoDatabaseName));

builder.Services.AddSingleton<IMarketRealtimePublisher, SignalRMarketRealtimePublisher>();

builder.Services.Configure<MarketDataOptions>(builder.Configuration.GetSection("MarketData"));

//Services
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<ILocalAiContextService, LocalAiContextService>();
builder.Services.AddScoped<IMarketCandleService, MarketCandleService>();
builder.Services.AddScoped<IMarketIngestionPipeline, MarketIngestionPipeline>();
builder.Services.AddScoped<IMarketQuoteService, MarketQuoteService>();
builder.Services.AddScoped<IMarketReferencePipeline, MarketReferencePipeline>();
builder.Services.AddScoped<IMarketSnapshotProjector, MarketSnapshotProjector>();
builder.Services.AddScoped<IMarketSnapshotService, MarketSnapshotService>();
builder.Services.AddScoped<IMarketTradeService, MarketTradeService>();
builder.Services.AddScoped<IMessageCorrelationService, MessageCorrelationService>();
builder.Services.AddScoped<IMessageTriageService, MessageTriageService>();
builder.Services.AddScoped<IProfanityService, ProfanityService>();
builder.Services.AddScoped<IProfanityAdminService, ProfanityAdminService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<ITechnicalIndicatorCalculator, TechnicalIndicatorCalculator>();
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
builder.Services.AddSingleton<IMistralRequestRegistry, MistralRequestRegistry>();
builder.Services.AddSingleton<IMistralBackgroundQueue, MistralBackgroundQueue>();

builder.Services.AddScoped<EconomicTrendsDomainGuard>();
builder.Services.AddScoped<MistralOrchestrator>();
builder.Services.AddScoped<IMistralOrchestrator>(sp => sp.GetRequiredService<MistralOrchestrator>());
builder.Services.AddScoped<IMistralQueuedRequestProcessor>(sp => sp.GetRequiredService<MistralOrchestrator>());
builder.Services.AddScoped<IMistralInteractionRepository, MistralInteractionsRepository>();
builder.Services.AddScoped<IMistralInteractionNoSqlRepository, MistralInteractionNoSqlRepository>();
builder.Services.AddScoped<IProviderInstrumentRepository, ProviderInstrumentRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProfanityRepository, ProfanityRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ITechnicalIndicatorRepository, TechnicalIndicatorRepository>();
builder.Services.AddScoped<IUserMessageAdminQueueRepository, UserMessageAdminQueueRepository>();
builder.Services.AddScoped<IUserMessageRepository, UserMessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSessionsRepository, UserSessionRepository>();

builder.Services.AddScoped<IPasswordHasher<Users>, Argon2PasswordHasher>();

builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<IMarketDataSource, MockMarketDataSource>();

builder.Services.AddHostedService<MistralBackgroundWorker>();
builder.Services.AddHostedService<MarketIngestionHostedService>();

builder.Services.AddHttpClient<IGenerativeAiService, OllamaGenerativeAiService>(
(sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["MistralAI:ApiUrl"] ?? "http://127.0.0.1:11434/";

    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");

    client.Timeout = TimeSpan.FromSeconds(config.GetValue<int?>("MistralAI:TimeoutSeconds") ?? 180);
});

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is missing.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),

                ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),

                ValidIssuer = jwtIssuer,

                ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),

                ValidAudience = jwtAudience,

                ValidateLifetime = true,
                RequireExpirationTime = true,

                ClockSkew = TimeSpan.FromMinutes(2)
            };

        options.Events =
            new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var path = context.HttpContext.Request.Path;
                    var queryToken = context.Request.Query["access_token"];

                    if (path.StartsWithSegments("/hubs", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(queryToken))
                    {
                        context.Token = queryToken;
                    }

                    return Task.CompletedTask;
                }
            };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
       "User",
       policy =>
       {
           policy.RequireAuthenticatedUser();

           policy.RequireRole(Roles.User, Roles.Moderator, Roles.Admin);
       });
    options.AddPolicy(
        "Admin",
        policy =>
            policy.RequireRole(Roles.Admin));

    options.AddPolicy(
        "AdminOrModo",
        policy =>
            policy.RequireRole(Roles.Admin, Roles.Moderator));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<MarketDataHub>("/hubs/market-data");

app.MapHub<UserHub>($"/{UserHubMethods.HubPath}");

app.MapGet("/_diag/db", async () =>
{
    await using var connection = new SqlConnection(connectionString);

    await connection.OpenAsync();

    await using var command = connection.CreateCommand();

    command.CommandText = @"
                        SELECT
                            @@SERVERNAME AS ServerName,
                            DB_NAME() AS DatabaseName,
                            (
                                SELECT COUNT(*)
                                FROM dbo.Users
                            ) AS UserCount;
        
                        ";

    await using var reader = await command.ExecuteReaderAsync();

    await reader.ReadAsync();

    return Results.Ok(new
    {
        ServerName = reader["ServerName"]?.ToString(),
        DatabaseName = reader["DatabaseName"]?.ToString(),
        UserCount = Convert.ToInt32(reader["UserCount"])
    });
})
.AllowAnonymous();

app.Run();



































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.