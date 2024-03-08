using AutenticationBlazorWebApi.Client.Helpers;
using AutenticationBlazorWebApi.Client.Services.Contracts;
using AutenticationBlazorWebApi.Server;

namespace AutenticationBlazorWebApi.Client.Services.Implementations
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly GetHttpClient _gethttpClient;

        public WeatherForecastService(GetHttpClient gethttpClient)
        {
            _gethttpClient = gethttpClient;
        }
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
        {
            var httpClient = _gethttpClient.GetPrivateHttpClient();
            try
            {
                var result = await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast/WeatherForecast");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<WeatherForecast>();
            }
        }

        public async Task<IEnumerable<string>> GetSumaries()
        {
            var httpClient = _gethttpClient.GetPrivateHttpClient();
            try
            {
                var result = await httpClient.GetFromJsonAsync<IEnumerable<string>>("WeatherForecast/Summaries");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string>();
            }
        }
    }
}
