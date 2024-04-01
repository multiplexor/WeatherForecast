using WeatherForecast.Clients.OpenWeatherMapClient;
using WeatherForecast.Interfaces;

namespace WeatherForecast.Services
{
    public class OpenWeatherMapService : IWeatherForecastService
    {
        private readonly IOpenWeatherMapClient _openWeatherMapClient;

        public OpenWeatherMapService(IOpenWeatherMapClient openWeatherMapClient)
        {
            _openWeatherMapClient = openWeatherMapClient;
        }

        public async Task<Models.Forecast?> GetWeatherForecast(DateTime date, double latitude, double longitude)
        {
            var forecast = await _openWeatherMapClient.GetWeatherForecast(latitude, longitude);
            if (forecast == null)
                return null;

            var forecastForDate = forecast.daily.FirstOrDefault(f => DateTimeOffset.FromUnixTimeSeconds(f.dt).DateTime.Date == date.Date);
            if (forecastForDate == null)
                return null;

            return new Models.Forecast() { Provider = this.GetType().Name, Temperature_max = forecastForDate.temp.max, Temperature_min = forecastForDate.temp.min };
        }
    }
}
