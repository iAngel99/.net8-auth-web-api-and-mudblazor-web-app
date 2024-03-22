using AutenticationBlazorWebApi.Client.States;
using AutenticationBlazorWebApi.Models.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AutenticationBlazorWebApi.Client.Helpers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Obtener el token
                var stringToken = MainStates.JwtToken;

                // Si el token es nulo o vacío, devolver un estado de autenticación anónimo
                if (string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

                // Deserializar el token en un objeto UserSession
                var deserializeToken = Serializations.DeserializeJsonString<AuthResponseDto>(stringToken);

                // Si la deserialización falla (resultando en null), devolver un estado de autenticación anónimo
                if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(anonymous));

                // Descifrar el token
                var getUserClaims = DecryptToken(deserializeToken.Token!);

                // Si el descifrado falla (resultando en null), devolver un estado de autenticación anónimo
                if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));

                // Establecer el ClaimsPrincipal
                var claimsPrincipal = SetClaimPrincipal(getUserClaims);

                // Devolver el estado de autenticación
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }

        }

        public async Task UpdateAuthenticationState(AuthResponseDto userSession)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (userSession.Token != null || userSession.RefreshToken != null)
            {
                var serializeSession = Serializations.SerializeObj(userSession);
                MainStates.JwtToken = serializeSession;
                var getUserClaims = DecryptToken(userSession.Token!);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                MainStates.JwtToken = null!;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }


        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            // Si el token es nulo o vacío, devolver un nuevo objeto CustomUserClaims
            if (String.IsNullOrEmpty(jwtToken)) return new CustomUserClaims();

            // Crear un nuevo manejador de tokens JWT
            var handler = new JwtSecurityTokenHandler();

            // Leer el token JWT
            var token = handler.ReadJwtToken(jwtToken);

            // Extraer las afirmaciones del token
            var userId = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
            var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
            var lastName = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.GivenName);
            var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role);

            // Devolver un nuevo objeto CustomUserClaims con las afirmaciones extraídas
            return new CustomUserClaims(userId!.Value!, name!.Value!, lastName.Value!, email!.Value!, role!.Value!);
        }

        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (claims.Email is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, claims.Id),
                    new(ClaimTypes.Name, claims.Name),
                    new(ClaimTypes.Email, claims.Email),
                    new(ClaimTypes.GivenName,claims.GivenName),
                    new(ClaimTypes.Role, claims.Role)
                }, "JwtAuth"));
        }

    }
}
