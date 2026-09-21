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
        public string PasswordHashV2 { get; set; }
        public Guid SecurityStamp { get; set; } = Guid.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public UserStatus Status { get; set; }
        public bool Active { get; private set; } = true;

        // Particularly useful for Dapper / standard creation
        public Users()
        {
        }

        // Rehydration of an existing user
        public Users(
            int id,
            string email,
            string? passwordHashV2,
            Guid securityStamp,
            UserRole role,
            UserStatus status)
        {
            Id = id;
            Email = email;
            PasswordHashV2 = passwordHashV2;
            SecurityStamp = securityStamp;
            Role = role;
            Status = status;
        }
        public void Activate() => Active = true;
        public void Deactivate() => Active = false;
    }
}









































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.