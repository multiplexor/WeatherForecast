using WeatherForecast.Models;

namespace WeatherForecast.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<Forecast?> GetWeatherForecast(DateTime date, double latitude, double longitude);
    }
}