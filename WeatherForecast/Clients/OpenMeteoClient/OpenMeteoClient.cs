using System.Net.Http.Headers;
using System.Text.Json;
using WeatherForecast.Models;

namespace WeatherForecast.Clients.OpenMeteoClient
{
    public class OpenMeteoClient : IWeatherClient
    {
        private readonly HttpClient _httpClient;

        public OpenMeteoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Forecast?> GetWeatherForecast(DateTime date, double latitude, double longitude)
        {
            var response = await _httpClient.GetAsync($"forecast?latitude={latitude}&longitude={longitude}&daily=temperature_2m_max,temperature_2m_min");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var forecastResponse = JsonSerializer.Deserialize<Models.ForecastResponse>(content);
            if (forecastResponse?.daily == null)
                return null;

            var forecasts = forecastResponse.daily;
            int index = forecasts.time?.FindIndex(d => d.Date == date.Date) ?? -1;
            if (index == -1)
                return null;

            double temperatureMax = forecasts.temperature_2m_max[index];
            double temperatureMin = forecasts.temperature_2m_min[index];

            return new Forecast() { provider = this.GetType().Name, temperature_max = temperatureMax, temperature_min = temperatureMin };
        }
    }
}
