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

    }
}