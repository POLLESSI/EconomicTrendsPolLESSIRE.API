using EconomicTrendsPolLESSIRE.API.Tools;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using EconomicTrendsPolLESSIRE.Infrastructure.Repositories;
using EconomicTrendsPolLESSIRE.Infrastructure.Security;
using EconomicTrendsPolLESSIRE.Infrastructure.Services;
using EconomicTrendsPolLESSIRE.Shared.StaticConfig.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Text.Json.Serialization;
using System.Data;
using System.Text;

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
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
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
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ITechnicalIndicatorRepository, TechnicalIndicatorRepository>();
builder.Services.AddScoped<IUserMessageAdminQueueRepository, UserMessageAdminQueueRepository>();
builder.Services.AddScoped<IUserMessageRepository, UserMessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSessionsRepository, UserSessionRepository>();

builder.Services.AddScoped<IPasswordHasher<Users>, Argon2PasswordHasher>();

builder.Services.AddSingleton<TokenGenerator>();

var jwtSecret =
    builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException(
        "Jwt:Secret is missing.");

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"];

var jwtAudience =
    builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSecret)),

                ValidateIssuer =
                    !string.IsNullOrWhiteSpace(jwtIssuer),

                ValidIssuer = jwtIssuer,

                ValidateAudience =
                    !string.IsNullOrWhiteSpace(jwtAudience),

                ValidAudience = jwtAudience,

                ValidateLifetime = true,
                RequireExpirationTime = true,

                ClockSkew =
                    TimeSpan.FromMinutes(2)
            };

        options.Events =
            new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var path =
                        context.HttpContext.Request.Path;

                    var queryToken =
                        context.Request.Query["access_token"];

                    if (path.StartsWithSegments(
                            "/hubs",
                            StringComparison.OrdinalIgnoreCase)
                        &&
                        !string.IsNullOrWhiteSpace(queryToken))
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
        "AdminOrModo",
        policy =>
            policy.RequireRole(
                Roles.Admin,
                Roles.Moderator));
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

app.Run();



































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.