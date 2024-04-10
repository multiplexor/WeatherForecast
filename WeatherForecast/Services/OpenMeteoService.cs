using WeatherForecast.Clients.OpenMeteoClient;
using WeatherForecast.Interfaces;
using WeatherForecast.Models;

namespace WeatherForecast.Services
{
    public class OpenMeteoService : IWeatherForecastService
    {
        private readonly IOpenMeteoClient _openMeteoClient;
        private readonly ILogger<OpenMeteoService> _logger;
        private const string _provider = "OpenMeteo";

        public OpenMeteoService(IOpenMeteoClient openMeteoClient, ILogger<OpenMeteoService> logger)
        {
            _openMeteoClient = openMeteoClient;
            _logger = logger;
        }

        public async Task<ForecastResponse?> GetWeatherForecast(DateTime date, double latitude, double longitude)
        {
            Clients.OpenMeteoClient.Models.ForecastResponse? forecastResponse;
            try
            {
                forecastResponse = await _openMeteoClient.GetWeatherForecast(latitude, longitude);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"unable to get data from {_provider}");
                return ForecastResponse.FailureResult(_provider, $"unable to get data from {_provider}");
            }
            if (forecastResponse?.daily == null)
                return ForecastResponse.FailureResult(_provider, "response doesn't contain forecast data");

            var forecasts = forecastResponse.daily;
            int index = forecasts.time?.FindIndex(d => d.Date == date.Date) ?? -1;
            if (index == -1)
                return ForecastResponse.FailureResult(_provider, "response doesn't contain forecast data for requested date");

            double temperatureMax = forecasts.temperature_2m_max[index];
            double temperatureMin = forecasts.temperature_2m_min[index];

            return ForecastResponse.SuccessResult(_provider, temperatureMin, temperatureMax);
        }
    }
}
