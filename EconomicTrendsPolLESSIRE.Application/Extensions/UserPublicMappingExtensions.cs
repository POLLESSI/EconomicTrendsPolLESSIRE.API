using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Extensions
{
    public static class UserPublicMappingExtensions
    {
        public static UserPublicDTO ToPublicDTO(
            this Users user)
        {
            ArgumentNullException.ThrowIfNull(user);

            return new UserPublicDTO
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.ToString(),
                Status = user.Status.ToString(),
                Active = user.Active
            };
        }
    }
}

















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.