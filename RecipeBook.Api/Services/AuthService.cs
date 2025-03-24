using Microsoft.AspNetCore.Identity;
using RecipeBook.Api.Models;
using System.Security.Claims;

namespace RecipeBook.Api.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<IdentityUser> GetCurrentUserAsync();
        string GetCurrentUserId();
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthService(UserManager<IdentityUser> userManager,
                           ITokenService tokenService,
                           IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded && registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
            }

            return identityResult;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user == null || !await userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                throw new Exception("Username or password wrong");
            }

            var roles = await userManager.GetRolesAsync(user);
            var jwtToken = tokenService.CreateJWTToken(user, roles.ToList());

            return new LoginResponseDto { JwtToken = jwtToken };
        }

        public async Task<IdentityUser> GetCurrentUserAsync()
        {
            var userClaims = httpContextAccessor.HttpContext?.User;
            if (userClaims == null)
                throw new Exception("HttpContext.User is null.");

            if (!userClaims.Identity.IsAuthenticated)
            {
                // Debug: wypisz wszystkie claimy
                var claimsDebug = string.Join(", ", userClaims.Claims.Select(c => $"{c.Type}: {c.Value}"));
                throw new Exception($"User is not authenticated1. Claims: {claimsDebug}");
            }

            var userId = userManager.GetUserId(userClaims);
            if (string.IsNullOrEmpty(userId))
            {
                var claimsDebug = string.Join(", ", userClaims.Claims.Select(c => $"{c.Type}: {c.Value}"));
                throw new Exception($"User ID not found in token claims. Claims: {claimsDebug}");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found in the database.");

            return user;
        }

        public string GetCurrentUserId()
        {
            var userClaims = httpContextAccessor.HttpContext?.User;
            if (userClaims == null)
                throw new Exception("HttpContext.User is null.");

            if (!userClaims.Identity.IsAuthenticated)
            {
                var claimsDebug = string.Join(", ", userClaims.Claims.Select(c => $"{c.Type}: {c.Value}"));
                throw new Exception($"User is not authenticated2. Claims: {claimsDebug}");
            }

            var userId = userManager.GetUserId(userClaims);
            if (string.IsNullOrEmpty(userId))
            {
                var claimsDebug = string.Join(", ", userClaims.Claims.Select(c => $"{c.Type}: {c.Value}"));
                throw new Exception($"User ID not found in token claims. Claims: {claimsDebug}");
            }

            return userId;
        }
    }
}