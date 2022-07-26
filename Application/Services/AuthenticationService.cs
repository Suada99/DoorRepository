using Application.Models;
using Application.Models.DTOs;
using Application.Services.Interfaces;
using Application.Services.Interfacess;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtConfig _jwtConfig;
        private readonly IJWTTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        public AuthenticationService(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IJWTTokenService jWTTokenService, IOptionsMonitor<JwtConfig> jwtConfig, IWorkContext workContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig.CurrentValue;
            _jwtTokenService = jWTTokenService;
            _workContext = workContext;
        }

        public async Task<CommandResult<User>> RegisterUserAsync(UserRegistrationDto user)
        {
            // Verify if requested user with requested username already exists
            var existingUsername = await _userManager.FindByNameAsync(user.Username);
            if (existingUsername != null)
            {
                return new CommandResult<User>
                {
                    Success = false,
                    CommandError = new CommandError
                    {
                        HttpCode = System.Net.HttpStatusCode.Conflict,
                        Code = "409",
                        Description = $"User with username {user.Username} already exits!"
                    }
                };
            }

            // Verify if requested user with requested email already exists
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return new CommandResult<User>
                {
                    Success = false,
                    CommandError = new CommandError
                    {
                        HttpCode = System.Net.HttpStatusCode.Conflict,
                        Code = "409",
                        Description = $"User with email {user.Email} already exits!"
                    }
                };
            }

            var mappedUser = _mapper.Map<User>(user);
            mappedUser.EmailConfirmed = true;

            var isCreated = await _userManager.CreateAsync(mappedUser, user.Password);
            if (!isCreated.Succeeded)
            {
                return new CommandResult<User>
                {
                    Success = false,
                    CommandError = new CommandError
                    {
                        HttpCode = System.Net.HttpStatusCode.InternalServerError,
                        Code = "500",
                        Description = $"Error raised during creating user!"
                    }
                };
            }
            return new CommandResult<User>
            {
                Success = true,
                Data = mappedUser
            };
        }

        public async Task<CommandResult<AuthResult>> LogInUserAsync(UserLoginRequest requestUser)
        {
            var existingUser = await _userManager.FindByEmailAsync(requestUser.Email);

            if (existingUser == null)
            {
                return new CommandResult<AuthResult>
                {
                    Success = false,
                    CommandError = new CommandError
                    {
                        HttpCode = System.Net.HttpStatusCode.NotFound,
                        Code = "404",
                        Description = $"User with email {requestUser.Email} does not exits!"
                    }
                };
            }

            var result = await _signInManager.PasswordSignInAsync(existingUser.UserName, requestUser.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return new CommandResult<AuthResult>
                {
                    Success = false,
                    CommandError = new CommandError
                    {
                        HttpCode = System.Net.HttpStatusCode.BadRequest,
                        Code = "400",
                        Description = $"User with email {requestUser.Email} couldn't log in!"
                    }
                };
            }

            var jwtToken = await GenerateJwtToken(existingUser);
            return new CommandResult<AuthResult>
            {
                Success = true,
                Data = jwtToken
            };
        }

        public async Task<CommandResult<bool>> LogOutUserAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                await _workContext.DeactivateCurrentTokenAsync();
            }
            catch (Exception e)
            {
                return new CommandResult<bool>
                {
                    Success = false,
                    CommandError = new CommandError
                    {
                        Description = e.Message,
                        Code = "500",
                        HttpCode = System.Net.HttpStatusCode.InternalServerError
                    }
                };
            }
            return new CommandResult<bool>
            {
                Success = true
            };

        }

        private async Task<AuthResult> GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.UserName),
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            var generatedToken = new JWTToken()
            {
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = tokenDescriptor.Expires.Value,
                Token = jwtToken
            };

            await _jwtTokenService.InsertJWTTokenAsync(generatedToken);

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true
            };
        }

    }
}
