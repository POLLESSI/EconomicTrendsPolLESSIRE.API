using EconomicTrendsPolLESSIRE.Contracts.Enums;

namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class UserSessions
    {
    #nullable disable
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string Jti { get; set; }
        public Guid RefreshFamilyId { get; set; }
        public DateTime IssueAtUtc { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public DateTime LastSeenUtc { get; set; }
        public int Source { get; set; }
        public string? Ip { get; set; }
        public string? UserAgent { get; set; }
        public bool IsRevoked { get; set; }
    }
}
