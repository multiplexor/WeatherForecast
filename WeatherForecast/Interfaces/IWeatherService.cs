using WeatherForecast.Models;
using WeatherForecast.Services.LocationService;

namespace WeatherForecast.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherForecastResponse> GetForecast(DateTime date, string city, string country);
    }
}