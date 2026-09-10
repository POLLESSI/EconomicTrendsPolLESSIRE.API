using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Users?> GetUserByEmailAsync(string email);
        Task<Users?> SaveUsersAsync(Users users);
        Task<IEnumerable<Users>> GetAllUsersAsync(int limit = 200, CancellationToken ct = default);
        Task<Users?> GetUsersByIdAsync(int id);
        Task<Users> RegisterUserAsync(Users user);
        Task DeactivateUserAsync(int id);
        Task AnonymizeUserAsync(int userId, CancellationToken ct = default);
        void SetRole(int id, string? role);
        Users? UpdateUser(Users user);
    }
}
