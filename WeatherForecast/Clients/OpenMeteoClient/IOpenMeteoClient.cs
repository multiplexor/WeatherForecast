using WeatherForecast.Clients.OpenMeteoClient.Models;

namespace WeatherForecast.Clients.OpenMeteoClient
{
    public interface IOpenMeteoClient
    {
        public Task<ForecastResponse?> GetWeatherForecast(double latitude, double longitude);
    }
}
