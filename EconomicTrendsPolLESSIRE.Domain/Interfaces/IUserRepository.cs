using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Users?> GetUserByEmailAsync(string email);
        Task<Users?> GetUserByIdAsync(int id);
        Task<IEnumerable<Users>> GetAllActiveUsersAsync();
        Task<Users> RegisterUserAsync(Users user);
        Task DeactivateUserAsync(int id);
        Task AnonymizeUserAsync(int userId, CancellationToken ct = default);
        void SetRole(int id, string? role);
        Users? UpdateUser(Users user);

        // =====================================================
        // PASSWORD HASH V2
        // =====================================================
        Task UpdatePasswordHashV2Async(int userId, string passwordHashV2);
    }
}















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.