using AutenticationBlazorWebApi.Server;

namespace AutenticationBlazorWebApi.Client.Services.Contracts
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecast();

        Task<IEnumerable<string>> GetSumaries();
    }
}
