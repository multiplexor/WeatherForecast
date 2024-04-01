using System.Net.Http.Headers;
using System.Text.Json;
using WeatherForecast.Clients.OpenWeatherMapClient.Models;

namespace WeatherForecast.Clients.OpenWeatherMapClient
{
    public class OpenWeatherMapClient : IOpenWeatherMapClient
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

        public async Task<Forecasts?> GetWeatherForecast(double latitude, double longitude)
        {
            var response = await _httpClient.GetAsync($"onecall?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric&exclude=current,minutely,hourly,alerts");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Forecasts>(content);
        }
    }
}
