
using WeatherForecast.Clients.WeatherbitClient;
using WeatherForecast.Clients.WeatherbitClient.Models;
using WeatherForecast.Interfaces;
using WeatherForecast.Models;

namespace WeatherForecast.Services
{
    public class WeatherbitService : IWeatherForecastService
    {
        private readonly IWeatherbitClient _weatherbitClient;
        private readonly ILogger<OpenMeteoService> _logger;
        private const string _provider = "weatherbit";

        public WeatherbitService(IWeatherbitClient weatherbitClient, ILogger<OpenMeteoService> logger)
        {
            _weatherbitClient = weatherbitClient;
            _logger = logger;
        }

        public async Task<ForecastResponse?> GetWeatherForecast(DateTime date, double latitude, double longitude)
        {
            Forecasts? forecastResponse;
            try
            {
                forecastResponse = await _weatherbitClient.GetWeatherForecast(latitude, longitude);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"unable to get data from {_provider}");
                return ForecastResponse.FailureResult(_provider, $"unable to get data from {_provider}");
            }
            if (forecastResponse?.data == null)
                return ForecastResponse.FailureResult(_provider, "response doesn't contain forecast data");

            var forecastForDate = forecastResponse.data.FirstOrDefault(f => f.valid_date.Date == date.Date);
            if (forecastForDate == null)
                return ForecastResponse.FailureResult(_provider, "response doesn't contain forecast data for requested date");

            return ForecastResponse.SuccessResult(_provider, forecastForDate.min_temp, forecastForDate.max_temp);
        }
    }
}
