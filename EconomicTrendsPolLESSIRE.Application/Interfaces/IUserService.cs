using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IUserService
    {
    #nullable disable
        Task<Users> AuthenticateAsync(string email, string password);
        Task<Users> GetUserByEmailAsync(string email);
        Task<Users> GetUserByIdAsync(int id);
        Task<IEnumerable<Users>> GetAllActiveUsersAsync();
        Task<UserDTO> RegisterUserAsync(string email, string password, UserRole role);
        Task<bool> LoginAsync(string email, string password);
        Task DeactivateUserAsync(int id);
        void SetRole(int id, string? role);
        Users? UpdateUser(Users user);
    }
}



















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.