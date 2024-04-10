using WeatherForecast.Models;

namespace WeatherForecast.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<ForecastResponse?> GetWeatherForecast(DateTime date, double latitude, double longitude);
    }
}