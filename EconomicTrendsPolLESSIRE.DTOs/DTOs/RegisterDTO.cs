using System.ComponentModel.DataAnnotations;

namespace EconomicTrendsPolLESSIRE.DTOs.DTOs
{
    public sealed class RegisterDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(64)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(12)]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;
    }
}
















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.