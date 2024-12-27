using AutenticationBlazorWebApi.Client.Services;
using AutenticationBlazorWebApi.Models.DTOs;

namespace AutenticationBlazorWebApi.Client.Helpers
{
    public class GetHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserSession _userSession;
        private const string HeaderKey = "Authorization";

        public GetHttpClient(IHttpClientFactory httpClientFactory, IUserSession userSession)
        {
            _httpClientFactory = httpClientFactory;
            _userSession = userSession;
        }


        public HttpClient GetPrivateHttpClient()
        {
            // Crear un nuevo cliente HTTP
            var client = _httpClientFactory.CreateClient("SystemApiClient");

            // Obtener el token del almacenamiento local
            var stringToken = _userSession.GetUserData<string>("Token");

            // Si el token es nulo o vacío, devolver el cliente
            if (String.IsNullOrEmpty(stringToken)) return client;

            // Deserializar el token en un objeto UserTokenDto
            var deserializeToken = Serializations.DeserializeJsonString<AuthResponseDto>(stringToken);

            // Si la deserialización falla (resultando en null), devolver el cliente
            if (deserializeToken == null) return client;

            // Establecer el encabezado de autorización con el token
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", deserializeToken.Token);

            // Devolver el cliente
            return client;
        }

        public HttpClient GetPublicHttpClient()
        {
            var client = _httpClientFactory.CreateClient("SystemApiClient");
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;
        }

    }
}
