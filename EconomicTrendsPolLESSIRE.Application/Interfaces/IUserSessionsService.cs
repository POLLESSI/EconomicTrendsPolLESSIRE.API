using EconomicTrendsPolLESSIRE.Contracts.Enums;
using Microsoft.AspNetCore.Http;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IUserSessionsService
    {
        Task TrackAccessTokenAsync(string accessToken, string email, SessionSource source, HttpContext http);
        Task TrackLoginAsync(string email, string jti, DateTime issuedAtUtc, DateTime expiresAtUtc, string? ip, string? userAgent, CancellationToken ct = default);
    }
}























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.