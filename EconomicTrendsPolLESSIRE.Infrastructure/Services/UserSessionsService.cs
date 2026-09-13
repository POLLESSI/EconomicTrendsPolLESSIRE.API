using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class UserSessionsService : IUserSessionsService
    {
        private readonly IUserSessionsRepository _repo;
        private readonly ILogger<UserSessionsService> _log;

        public UserSessionsService(
            IUserSessionsRepository repo,
            ILogger<UserSessionsService> log)
        {
            _repo = repo;
            _log = log;
        }
        public Task TrackAccessTokenAsync(string accessToken, string email, SessionSource source, HttpContext http)
        {
            throw new NotImplementedException();
        }

        public Task TrackLoginAsync(string email, string jti, DateTime issuedAtUtc, DateTime expiresAtUtc, string? ip, string? userAgent, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
