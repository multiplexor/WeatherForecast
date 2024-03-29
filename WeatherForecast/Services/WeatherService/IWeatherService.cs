using WeatherForecast.Models;

namespace WeatherForecast.Services.WeatherService
{
    public interface IWeatherService
    {
        Task<ForecastResponse?> GetForecast(DateTime date, string city, string country);
    }
}