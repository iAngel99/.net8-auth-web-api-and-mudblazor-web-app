using AutenticationBlazorWebApi.Models;
using AutenticationBlazorWebApi.Models.DTOs;
using AutenticationBlazorWebApi.Models.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AutenticationBlazorWebApi.Server.Authentication
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthManager> _logger;
        private readonly JwtSettings _jwtSettings;
        private User _user;
        private const string _loginProvider = "AutenticationWebApi.Server";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<User> userManager, IOptions<JwtSettings> jwtSettings, ILogger<AuthManager> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<GeneralResponse> Register(RegisterDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await AssignRoleToUser(user);
                // Si el registro es exitoso, devuelve un GeneralResponse con Flag = true y un mensaje de éxito.
                return new GeneralResponse(true, "Registrado con éxito");
            }
            else
            {
                // Si el registro falla, concatena todos los errores en un solo mensaje.
                string errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError(errorMessage);
                // Devuelve un GeneralResponse con Flag = false y el mensaje de error.
                return new GeneralResponse(false, "Un usuario con ese email ya está registrado");
            }
        }

        private async Task AssignRoleToUser(User user)
        {
            var hasUsers = await _userManager.Users.CountAsync();
            var role = hasUsers > 1 ? "User" : "Administrator";
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<LoginResponse> Login(LoginDto loginDto)
        {
            _user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

            if (_user == null || !isValidUser)
            {
                var msg = $"Usuario con email {loginDto.Email} o contraseña incorrecta";
                _logger.LogWarning(msg);
                return new LoginResponse(false, msg);
            }

            var token = await GenerateToken();
            var refreshToken = await CreateRefreshToken();
            return new LoginResponse(true, "Acceso exitoso", token, refreshToken);
        }

        public async Task<LoginResponse> VerifyRefreshToken(AuthResponseDto request)
        {
            _user = await GetUserFromToken(request.Token);

            if (_user == null)
            {
                // Devuelve un LoginResponse con Flag = false y un mensaje de error.
                return new LoginResponse(false, "User not found or invalid token.");
            }

            if (await IsRefreshTokenValid(request.RefreshToken))
            {
                var token = await GenerateToken();
                var refreshToken = await CreateRefreshToken();
                // Devuelve un LoginResponse con Flag = true, el nuevo token y el nuevo refresh token.
                return new LoginResponse(true, "Refresh token generated successfully.", token, refreshToken);
            }

            await _userManager.UpdateSecurityStampAsync(_user);
            // Devuelve un LoginResponse con Flag = false y un mensaje de error.
            return new LoginResponse(false, "Refresh token is invalid.");
        }



        public async Task<string> CreateRefreshToken()
        {
            await RemoveExistingRefreshToken();
            var newRefreshToken = await GenerateNewRefreshToken();
            await SetRefreshToken(newRefreshToken);
            return newRefreshToken;
        }

        private async Task RemoveExistingRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
        }

        private async Task<string> GenerateNewRefreshToken()
        {
            return await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
        }

        private async Task SetRefreshToken(string newRefreshToken)
        {
            await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
        }


        private async Task<User> GetUserFromToken(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(token);
            var username = tokenContent.Claims.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Email)?.Value;
            return await _userManager.FindByNameAsync(username);
        }

        private async Task<bool> IsRefreshTokenValid(string refreshToken)
        {
            return await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, refreshToken);
        }

        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = await GetClaimsForToken();

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_jwtSettings.DurationInMinutes)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<Claim>> GetClaimsForToken()
        {
            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(s => new Claim(ClaimTypes.Role, s)).ToList();
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.FirstName!),
                new Claim(ClaimTypes.GivenName, _user.LastName!),
                new Claim(ClaimTypes.Email, _user.Email),
                // Añade aquí más afirmaciones según sea necesario
            };

            return new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, _user.Email),
        }.Union(userClaims).Union(roleClaims).ToList();
        }
    }
}
