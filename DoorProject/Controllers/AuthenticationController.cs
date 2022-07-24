using AutoMapper;
using Core.Entities;
using Core.Services;
using DoorProject.Configurations;
using DoorProject.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoorProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IJWTTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;

        public AuthenticationController(SignInManager<User> signInManager, UserManager<User> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
             TokenValidationParameters tokenValidationParams,
            IJWTTokenService refreshTokenService,
            IMapper mapper,
            IWorkContext workContext
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParams = tokenValidationParams;
            _jwtTokenService = refreshTokenService;
            _mapper = mapper;
            _workContext = workContext;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                // We can utilise the model
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Email already in use"
                            },
                        Success = false
                    });
                }

                var mappedUser = _mapper.Map<User>(user);
                mappedUser.EmailConfirmed = true;
                var isCreated = await _userManager.CreateAsync(mappedUser, user.Password);
                if (isCreated.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var result = await _signInManager.PasswordSignInAsync(existingUser.UserName, user.Password, false, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var jwtToken = await GenerateJwtToken(existingUser);


                return Ok(jwtToken);
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }



        [Authorize]
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                await _workContext.DeactivateCurrentTokenAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("Error happened: " + e.Message);
            }
        }


        private async Task<AuthResult> GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
