using System.Net.Http.Headers;
using System.Text.Json;
using WeatherForecast.Clients.OpenWeatherMapClient.Models;

namespace WeatherForecast.Clients.OpenWeatherMapClient
{
    public class OpenWeatherMapClient : IWeatherClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenWeatherMapClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/3.0/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiKey = Environment.GetEnvironmentVariable("openweathermap-api-key") ?? string.Empty;
        }

        public async Task<WeatherForecast.Models.Forecast?> GetWeatherForecast(DateTime date, double latitude, double longitude)
        {
            var response = await _httpClient.GetAsync($"onecall?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric&exclude=current,minutely,hourly,alerts");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var forecast = JsonSerializer.Deserialize<Forecasts>(content);
            if (forecast == null)
                return null;

            var forecastForDate = forecast.daily.FirstOrDefault(f => DateTimeOffset.FromUnixTimeSeconds(f.dt).DateTime.Date == date.Date);
            if (forecastForDate == null)
                return null;

            return new WeatherForecast.Models.Forecast() { provider = this.GetType().Name, temperature_max = forecastForDate.temp.max, temperature_min = forecastForDate.temp.min };
        }
    }
}
