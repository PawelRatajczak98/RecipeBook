using Microsoft.AspNetCore.Identity;
using RecipeBook.Api.Entities;
using RecipeBook.Api.Models;
using AutoMapper;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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
            var user = new AppUser
            {
                UserName = registerRequestDto.Username,
                PasswordHash = registerRequestDto.Password,
            };
            
            var result = await userManager.CreateAsync(user, registerRequestDto.Password);
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Member");
            }
            return result;
        }
        
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByNameAsync(loginRequestDto.Username);
            if (user == null || !await userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                throw new Exception("Username or password wrong");
            }
            string jwtToken = await tokenService.CreateJWTToken(user);

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