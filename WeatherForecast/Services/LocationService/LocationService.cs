using System.Net.Http.Headers;
using System.Text.Json;
using WeatherForecast.Services.LocationService.Models;

namespace WeatherForecast.Services.LocationService
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        public LocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/v1");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<(double latitude, double longitude)?> GetLocation(string city, string country)
        {
            var response = await _httpClient.GetAsync($"search?name={city}&count=10&language=en&format=json");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var locations = JsonSerializer.Deserialize<LocationsList>(content);
            if (locations == null)
                return null;

            var location = locations.results.FirstOrDefault(l => l.country == country);
            if (location == null)
                return null;

            return (location.latitude, location.longitude);
        }
    }
}