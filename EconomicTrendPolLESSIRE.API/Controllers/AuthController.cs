using EconomicTrendsPolLESSIRE.API.Tools;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.DTOs.DTOs;
using EconomicTrendsPolLESSIRE.Shared.StaticConfig.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EconomicTrendsPolLESSIRE.API.Controllers
{
    [EnableRateLimiting("per-user")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserSessionsService _userSessionService;

        public AuthController(IUserService userService, ILogger<AuthController> logger, TokenGenerator tokenGenerator, IRefreshTokenService refreshTokenService, IUserSessionsService userSessionService)
        {
            _userService = userService;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _refreshTokenService = refreshTokenService;
            _userSessionService = userSessionService;
        }

        // -----------------------------
        // LOGIN
        // -----------------------------
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user is null)
            {
                _logger.LogWarning("Login attempt failed for {Email}", request.Email);
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var accessToken = _tokenGenerator.GenerateToken(user.Email, user.Role);
            var refreshToken = await _refreshTokenService.GenerateAsync(user.Email);

            // ---- SESSION TRACKING ----
            try
            {
                await _userSessionService.TrackAccessTokenAsync(
                    accessToken, user.Email, SessionSource.Api, HttpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Session tracking failed at login for {Email}", user.Email);
                // The login does not fail.
            }
            Response.Cookies.Append(Cookies.JwtTokenName, accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                Path = "/"
            });
            _logger.LogInformation("User {Email} logged in successfully", user.Email);
            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken.Token });
        }

        //// -----------------------------
        //// LOGOUT
        //// -----------------------------
        
        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutSessionRequest request)
        {
            if (request is not null && !string.IsNullOrWhiteSpace(request.Email) && !string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                try
                {
                    await _refreshTokenService.InvalidateAsync(request.RefreshToken.Trim(), request.Email.Trim());
                }
                catch (Exception ex)
                {
                    /*
                     * Don't expose whether a refresh token
                     * exists.
                     */
                    _logger.LogWarning(ex, "Refresh-token revocation failed during logout.");
                }
            }

            Response.Cookies.Delete(Cookies.JwtTokenName,
                new CookieOptions
                {
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/"
                });

            return Ok(
                new
                {
                    Message = "Logged out."
                });
        }

        //// -----------------------------
        //// REGISTER
        //// -----------------------------
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserPublicDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (dto is null)
            {
                return BadRequest(
                    new
                    {
                        Message = "Invalid registration request."
                    });
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(
                    new
                    {
                        Message = "Email and password are required."
                    });
            }

            var email = dto.Email.Trim();

            var existing = await _userService.GetUserByEmailAsync(email);

            if (existing is not null)
            {
                /*
                 * Do not allow the caller to choose
                 * an administrative role.
                 */
                return Conflict(
                    new
                    {
                        Message =
                            "If registration is possible for this address, " +
                            "the appropriate instructions have been processed."
                    });
            }

            /*
             * SECURITY:
             * Public registration always creates
             * a normal User account.
             */
            var userDto = await _userService.RegisterUserAsync(email, dto.Password, UserRole.User);

            _logger.LogInformation("New user registered. UserId={UserId}", userDto.Id);

            var result = new UserPublicDTO
            {
                Id = userDto.Id,
                Email = userDto.Email,
                Role = userDto.Role,
                Status = userDto.Status,
                Active = userDto.Active
            };

            return StatusCode(StatusCodes.Status201Created, result);
        }

        //// -----------------------------
        //// REFRESH
        //// -----------------------------
        [AllowAnonymous]
            [HttpPost("refresh")]
            public async Task<IActionResult> Refresh([FromBody] RefreshSessionRequest request)
            {
                if (request is null ||
                    string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized(
                        new
                        {
                            Message = "Invalid or expired session."
                        });
                }

                var email = request.Email.Trim();

                var refreshToken = request.RefreshToken.Trim();

                //    // ---------------------------------------------------------
                //    // 1. Validate current refresh token
                //    // ---------------------------------------------------------

                var valid = await _refreshTokenService.ValidateAsync(refreshToken, email);

                if (!valid)
                {
                    _logger.LogWarning("Refresh token rejected for {Email}", email);

                    return Unauthorized(
                        new
                        {
                            Message = "Invalid or expired session."
                        });
                }

                //    // ---------------------------------------------------------
                //    // 2. User must still be authorized to use OutZen
                //    // ---------------------------------------------------------

                var user = await _userService.GetUserByEmailAsync(email);

                if (user is null || !user.Active || user.Status != UserStatus.Active)
                {
                    /*
                     * Revoke the token if the account
                     * became unavailable.
                     */
                    await _refreshTokenService.InvalidateAsync(refreshToken, email);

                    return Unauthorized(
                        new
                        {
                            Message = "Invalid or expired session."
                        });
                }

                //    // ---------------------------------------------------------
                //    // 3. Consume previous refresh token
                //    // ---------------------------------------------------------

                await _refreshTokenService.InvalidateAsync(refreshToken, email);

                //    // ---------------------------------------------------------
                //    // 4. Generate a new pair
                //    // ---------------------------------------------------------

                var newAccessToken = _tokenGenerator.GenerateToken(user.Email, user.Role);

                var newRefreshToken = await _refreshTokenService.GenerateAsync(user.Email);

                //    // ---------------------------------------------------------
                //    // 5. Session tracking
                //    // ---------------------------------------------------------

                try
                {
                    await _userSessionService.TrackAccessTokenAsync(newAccessToken, user.Email, SessionSource.Api, HttpContext);
                }
                catch (Exception ex)
                {
                    /*
                     * Tracking failure does NOT invalidate
                     * an otherwise successful refresh.
                     */
                    _logger.LogError(ex, "Session tracking failed at refresh for {Email}", user.Email);
                }

                _logger.LogInformation("Access token refreshed for {Email}", user.Email);

                //    // ---------------------------------------------------------
                //    // 6. Return rotated pair
                //    // ---------------------------------------------------------

                return Ok(new TokenPairResponse
                {
                    AccessToken = newAccessToken,

                    RefreshToken = newRefreshToken.Token
                });
            }
        }
    }




































































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.