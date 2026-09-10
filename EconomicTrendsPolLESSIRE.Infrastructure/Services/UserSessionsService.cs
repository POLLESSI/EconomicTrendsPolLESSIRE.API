using EconomicTrendsPolLESSIRE.Application.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class UserSessionsService : IUserSessionsService
    {
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
