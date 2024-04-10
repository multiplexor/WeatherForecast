using System.Net.Http.Headers;
using System.Text.Json;
using WeatherForecast.Clients.WeatherbitClient.Models;

namespace WeatherForecast.Clients.WeatherbitClient
{
    public class WeatherbitClient : IWeatherbitClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherbitClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.weatherbit.io/v2.0/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiKey = Environment.GetEnvironmentVariable("weatherbit-api-key") ?? string.Empty;
        }

        public async Task<Forecasts?> GetWeatherForecast(double latitude, double longitude)
        {
            var response = await _httpClient.GetAsync($"forecast/daily?lat={latitude}&lon={longitude}&key={_apiKey}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Forecasts>(content);
        }
    }
}
