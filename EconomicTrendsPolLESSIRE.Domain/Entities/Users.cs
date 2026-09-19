using EconomicTrendsPolLESSIRE.Contracts.Enums;

namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class Users
    {
#nullable disable
        public int Id { get; private set; }
        public string Email { get; set; } = string.Empty;
        /*
         * Current ASP.NET Core Identity hash.
         */
        public string? PasswordHashV2 { get; set; }
        public Guid SecurityStamp { get; set; } = Guid.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public UserStatus Status { get; set; }
        public bool Active { get; private set; } = true;
        public void Activate() => Active = true;
        public void Deactivate() => Active = false;
    }
}









































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.