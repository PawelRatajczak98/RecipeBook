using Microsoft.AspNetCore.Identity;
using RecipeBook.Api.Entities;
using RecipeBook.Api.Models;
using AutoMapper;
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
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public AuthService(UserManager<AppUser> userManager,
                           ITokenService tokenService,
                           IHttpContextAccessor httpContextAccessor,
                           IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper=mapper;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var user = new AppUser();

            user.UserName = registerRequestDto.Username.ToLower();
            
            var result = await userManager.CreateAsync(user, registerRequestDto.Password);

            return result;
        }
        
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var normalizedUsername = loginRequestDto.Username.ToLowerInvariant();
            var user = await userManager.FindByNameAsync(normalizedUsername);
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