using EconomicTrendsPolLESSIRE.Application.Extensions;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class UserService : IUserService
    {
    #nullable disable
        private readonly IUserRepository _userRepository;
        private readonly IUserHubService _hubService;
        private readonly IPasswordHasher<Users> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IUserHubService hubService, IPasswordHasher<Users> passwordHasher, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _hubService = hubService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Users?> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("Authentication rejected: empty credentials.");

                return null;
            }

            var normalizedEmail = email.Trim();
            var user = await _userRepository.GetUserByEmailAsync(normalizedEmail);

            if (user is null)
            {
                _logger.LogWarning("Authentication rejected: user not found for {Email}.", normalizedEmail);

                return null;
            }

            if (!user.Active)
            {
                _logger.LogWarning("Authentication rejected: user {Email} is inactive.", normalizedEmail);

                return null;
            }

            if (user.Status != UserStatus.Active)
            {
                _logger.LogWarning("Authentication rejected: user {Email} has status {Status}.", normalizedEmail, user.Status);

                return null;
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHashV2))
            {
                _logger.LogWarning("Authentication rejected: user {Email} has no password hash.", normalizedEmail);

                return null;
            }

            var verification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHashV2, password);

            _logger.LogInformation("Password verification for {Email}: {Result}.", normalizedEmail, verification);

            if (verification == PasswordVerificationResult.Failed)
            {
                return null;
            }

            if (verification == PasswordVerificationResult.SuccessRehashNeeded)
            {
                var rehashed = _passwordHasher.HashPassword(user, password);

                await _userRepository.UpdatePasswordHashV2Async( user.Id, rehashed);

                user.PasswordHashV2 = rehashed;
            }

            return user;
        }

        public async Task DeactivateUserAsync(int id)
        {
            await _userRepository.DeactivateUserAsync(id);
            await _hubService.NotifyUserDeactivated(id);
        }

        public async Task<IEnumerable<Users>> GetAllActiveUsersAsync()
            => await _userRepository.GetAllActiveUsersAsync();

        public async Task<Users> GetUserByEmailAsync(string email)
            => await _userRepository.GetUserByEmailAsync(email);

        public async Task<Users> GetUserByIdAsync(int id)
            => await _userRepository.GetUserByIdAsync(id);

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await AuthenticateAsync(email, password);

            return user is not null;
        }

        public async Task<UserDTO> RegisterUserAsync(string email, string password, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(password));
            }

            email = email.Trim();

            var existing = await _userRepository.GetUserByEmailAsync(email);

            if (existing is not null)
            {
                throw new InvalidOperationException($"User '{email}' already exists.");
            }

            var newUser =
                new Users
                {
                    Email = email,
                    Role = role,
                    SecurityStamp = Guid.NewGuid(),
                    Status = UserStatus.Active,
                };

            newUser.Activate();

            newUser.PasswordHashV2 = _passwordHasher.HashPassword(newUser, password);

            var created = await _userRepository.RegisterUserAsync(newUser);

            await _hubService.NotifyUserRegistered(created.Email);

            return created.MapToUserDTO();
        }

        public void SetRole(int id, string role)
        {
            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole))
                throw new ArgumentException("Invalid role format", nameof(role));

            _userRepository.SetRole(id, parsedRole.ToString());
        }

        public Users? UpdateUser(Users user)
            => _userRepository.UpdateUser(user);
    }
}





















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.