using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class UserService : IUserService
    {
        public Task<Users> AuthenticateAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task DeactivateUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Users>> GetAllActiveUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> RegisterUserAsync(string email, string password, UserRole role)
        {
            throw new NotImplementedException();
        }

        public void SetRole(int id, string? role)
        {
            throw new NotImplementedException();
        }

        public Users? UpdateUser(Users user)
        {
            throw new NotImplementedException();
        }
    }
}
