using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Dapper;
using System.Data;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
            // Type handler to properly map UserRole <-> int
            SqlMapper.AddTypeHandler(new RoleTypeHandler());
        }
        public Task AnonymizeUserAsync(int userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeactivateUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Users>> GetAllUsersAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Users?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Users?> GetUsersByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Users> RegisterUserAsync(Users user)
        {
            throw new NotImplementedException();
        }

        public Task<Users?> SaveUsersAsync(Users users)
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
