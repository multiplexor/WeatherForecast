using WeatherForecast.Models;

namespace WeatherForecast.Clients
{
    public interface IWeatherClient
    {
        Task<Forecast?> GetWeatherForecast(DateTime date, double latitude, double longitude);
    }
}