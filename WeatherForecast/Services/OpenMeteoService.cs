using WeatherForecast.Clients.OpenMeteoClient;
using WeatherForecast.Interfaces;
using WeatherForecast.Models;

namespace WeatherForecast.Services
{
    public class OpenMeteoService : IWeatherForecastService
    {
        private readonly IOpenMeteoClient _openMeteoClient;

        public OpenMeteoService(IOpenMeteoClient openMeteoClient)
        {
            _openMeteoClient = openMeteoClient;
        }

        public async Task<Forecast?> GetWeatherForecast(DateTime date, double latitude, double longitude)
        {
            var forecastResponse = await _openMeteoClient.GetWeatherForecast(latitude, longitude);
            if (forecastResponse?.daily == null)
                return null;

            var forecasts = forecastResponse.daily;
            int index = forecasts.time?.FindIndex(d => d.Date == date.Date) ?? -1;
            if (index == -1)
                return null;

            double temperatureMax = forecasts.temperature_2m_max[index];
            double temperatureMin = forecasts.temperature_2m_min[index];

            return new Forecast() { Provider = this.GetType().Name, Temperature_max = temperatureMax, Temperature_min = temperatureMin };
        }
    }
}
