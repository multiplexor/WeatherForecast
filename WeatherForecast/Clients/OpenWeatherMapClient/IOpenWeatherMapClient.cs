using WeatherForecast.Clients.OpenWeatherMapClient.Models;

namespace WeatherForecast.Clients.OpenWeatherMapClient
{
    public interface IOpenWeatherMapClient
    {
        Task<Forecasts?> GetWeatherForecast(double latitude, double longitude);
    }
}
