using WeatherForecast.Clients.OpenWeatherMapClient;
using WeatherForecast.Clients.OpenWeatherMapClient.Models;
using WeatherForecast.Interfaces;
using WeatherForecast.Models;

namespace WeatherForecast.Services
{
    public class OpenWeatherMapService : IWeatherForecastService
    {
        private readonly IOpenWeatherMapClient _openWeatherMapClient;
        private readonly ILogger<OpenWeatherMapService> _logger;
        private const string _provider = "OpenWeatherMap";

        public OpenWeatherMapService(IOpenWeatherMapClient openWeatherMapClient, ILogger<OpenWeatherMapService> logger)
        {
            _openWeatherMapClient = openWeatherMapClient;
            _logger = logger;
        }

        public async Task<ForecastResponse?> GetWeatherForecast(DateTime date, double latitude, double longitude)
        {
            Forecasts? forecast;
            try
            {
                forecast = await _openWeatherMapClient.GetWeatherForecast(latitude, longitude);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"unable to get data from {_provider}");
                return ForecastResponse.FailureResult(_provider, $"unable to get data from {_provider}");
            }

            var forecastForDate = forecast?.daily.FirstOrDefault(f => DateTimeOffset.FromUnixTimeSeconds(f.dt).DateTime.Date == date.Date);
            if (forecastForDate == null)
                return ForecastResponse.FailureResult(_provider, "No forecast data for requested date.");

            return ForecastResponse.SuccessResult(_provider, forecastForDate.temp.min, forecastForDate.temp.max);
        }
    }
}
