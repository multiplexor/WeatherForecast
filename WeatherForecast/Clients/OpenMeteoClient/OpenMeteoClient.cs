using System.Net.Http.Headers;
using System.Text.Json;
using WeatherForecast.Clients.OpenMeteoClient.Models;

namespace WeatherForecast.Clients.OpenMeteoClient
{
    public class OpenMeteoClient : IOpenMeteoClient
    {
        private readonly HttpClient _httpClient;

        public OpenMeteoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ForecastResponse?> GetWeatherForecast(double latitude, double longitude)
        {
            var response = await _httpClient.GetAsync($"forecast?latitude={latitude}&longitude={longitude}&daily=temperature_2m_max,temperature_2m_min");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ForecastResponse>(content);
        }
    }
}
