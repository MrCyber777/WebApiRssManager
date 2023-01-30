using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiRssManager_Main.Helpers;

namespace WebApiRssManager_Main.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApiSettings _apiSettings;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,  IOptions<ApiSettings> options)
        {
            _signInManager = signInManager;
            _userManager = userManager;          
            _apiSettings = options.Value;
        }
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] UserRequestDTO userRequestDTO)
        {
            if (userRequestDTO == null || !ModelState.IsValid)
                return BadRequest();

            //Register a new user
            //var user = new ApplicationUser();
            //user.
            ApplicationUser user = new ApplicationUser
            {
                FirstName = userRequestDTO.FirstName,
                LastName = userRequestDTO.LastName,
                UserName = userRequestDTO.Email,
                Email = userRequestDTO.Email,
                PhoneNumber = userRequestDTO.Phone,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, userRequestDTO.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return BadRequest(new RegistrationResponseDTO { IsRegistrationSuccessfull = false, RegistrationErrors = errors });
            }                    
            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] AuthenticationRequestDTO authenticationRequestDTO)
        {

            var result = await _signInManager.PasswordSignInAsync(authenticationRequestDTO.UserName, authenticationRequestDTO.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(authenticationRequestDTO.UserName);
                if (user is null)
                {
                    return Unauthorized(new AuthenticationResponseDTO
                    {
                        ErrorMessage = "Invalid authentication!",
                        IsAuthenticationSuccessfull = false
                    });
                }
                var signInCredentials = GetSignInCredentials();
                var claims = await GetClaims(user);
                var tokenOptions = new JwtSecurityToken(
                    issuer: _apiSettings.ValidIssuer,
                    audience: _apiSettings.ValidAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(30),
                    signingCredentials: signInCredentials
                    );
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new AuthenticationResponseDTO
                {
                    IsAuthenticationSuccessfull = true,
                    Token = "Bearer "+token,
                    User = new UserDTO
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Phone = user.PhoneNumber,
                        Id = user.Id
                    }
                });
            }
            else
            {
                return Unauthorized(new AuthenticationResponseDTO
                {
                    ErrorMessage = "Invalid authentication!",
                    IsAuthenticationSuccessfull = false
                });
            }

        }
        SigningCredentials GetSignInCredentials()//Метод генерации ключей 
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
            var keyResult = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            return keyResult;
        }
        async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("Id",user.Id)
            };           
            return claims;
        }
    }
}
