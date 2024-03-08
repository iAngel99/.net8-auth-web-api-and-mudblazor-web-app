using AutenticationBlazorWebApi.Client.Services.Contracts;
using AutenticationBlazorWebApi.Client.States;
using AutenticationBlazorWebApi.Models.DTOs;
using System.Net;
using System.Net.Http.Headers;

namespace AutenticationBlazorWebApi.Client.Helpers
{
    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly IUserAccountService _userAccountService;

        public CustomHttpHandler(IUserAccountService userAccountService)
        {

            _userAccountService = userAccountService;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri.AbsoluteUri.Contains("login");
            bool registerUrl = request.RequestUri.AbsoluteUri.Contains("register");
            bool refreshTokenUrl = request.RequestUri.AbsoluteUri.Contains("refreshtoken");

            if (loginUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken);

            var result = await base.SendAsync(request, cancellationToken);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    // Get token from localStorage
                    var stringtoken = MainStates.JwtToken;
                    if (stringtoken == null) return result;
                    // Check if the header contain token
                    string token = string.Empty;
                    if (request.Headers.Authorization != null)
                    {
                        token = request.Headers.Authorization.Parameter;
                    }

                    var deserializedToken = Serializations.DeserializeJsonString<AuthResponseDto>(stringtoken);
                    if (deserializedToken is null) return result;

                    if (string.IsNullOrEmpty(token))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                        return await base.SendAsync(request, cancellationToken);
                    }

                    // Call for refresh token
                    var newJwtToken = await GetRefreshToken(deserializedToken);
                    if (string.IsNullOrEmpty(newJwtToken)) return result;

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newJwtToken);
                    return await base.SendAsync(request, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

            return result;
        }

        private async Task<string> GetRefreshToken(AuthResponseDto tokenDto)
        {
            var result = await _userAccountService.RefreshToken(tokenDto);
            string serializedToken = Serializations.SerializeObj(result);
            MainStates.JwtToken = serializedToken;
            return result.Token;
        }
    }
}
