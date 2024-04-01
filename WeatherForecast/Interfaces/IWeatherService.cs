using WeatherForecast.Models;

namespace WeatherForecast.Interfaces
{
    public interface IWeatherService
    {
        Task<ForecastResponse?> GetForecast(DateTime date, string city, string country);
    }
}