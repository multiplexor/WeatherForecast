using System;
using System.Net.Http.Headers;
using System.Text.Json;
using WeatherForecast.Interfaces;
using WeatherForecast.Services.LocationService.Models;

namespace WeatherForecast.Services.LocationService
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        public LocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/v1/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<LocationResponse> GetLocation(string city, string country)
        {
            var response = await _httpClient.GetAsync($"search?name={city}&count=10&language=en&format=json");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var locations = JsonSerializer.Deserialize<LocationsList>(content);
            if (locations == null)
                throw new Exception($"Location service returned no results");

            var location = locations.results.FirstOrDefault(l => l.country == country);
            if (location == null)
                throw new Exception($"Coordinates doesn't exist for city - {city}, country - {country}");

            return new LocationResponse { Latitude = location.latitude, Longitude = location.longitude };
        }
    }
}