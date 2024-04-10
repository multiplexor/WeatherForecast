using WeatherForecast.Clients.WeatherbitClient.Models;

namespace WeatherForecast.Clients.WeatherbitClient
{
    public interface IWeatherbitClient
    {
        Task<Forecasts?> GetWeatherForecast(double latitude, double longitude);
    }
}
