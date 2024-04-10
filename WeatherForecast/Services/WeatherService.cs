using Microsoft.Extensions.Caching.Memory;
using WeatherForecast.Interfaces;
using WeatherForecast.Models;

public class WeatherService : IWeatherService
{
    private readonly IMemoryCache _cache;
    private readonly IEnumerable<IWeatherForecastService> _weatherClients;
    private readonly ILogger<WeatherService> _logger;
    private readonly ILocationService _locationService;

    public WeatherService(IMemoryCache cache, IEnumerable<IWeatherForecastService> weatherClients, ILocationService locationService, ILogger<WeatherService> logger)
    {
        _cache = cache;
        _weatherClients = weatherClients;
        _logger = logger;
        _locationService = locationService;
    }

    public async Task<WeatherForecastResponse> GetForecast(DateTime date, string city, string country)
    {
        if (_weatherClients == null || !_weatherClients.Any())
        {
            _logger.LogWarning("No weather forecast services available.");
            throw new Exception("No weather forecast services available.");
        }

        var cacheKey = $"{date}-{city}-{country}";
        if (_cache.TryGetValue(cacheKey, out WeatherForecastResponse? cachedForecast) && cachedForecast != null)
        {
            _logger.LogInformation("Retrieved forecast from cache.");
            return cachedForecast;
        }

        var location = await _locationService.GetLocation(city, country);
        var forecastTasks = new List<Task<ForecastResponse?>>();
        foreach (var weatherClient in _weatherClients)
        {
            forecastTasks.Add(Task.Run(() => weatherClient.GetWeatherForecast(date, location.Latitude, location.Longitude)));
        }
        await Task.WhenAll(forecastTasks);

        List<ForecastResponse> forecasts = forecastTasks.Select(task => task.Result).ToList();
        var forecastResponse = new WeatherForecastResponse
        {
            Date = date,
            City = city,
            Country = country,
            Forecast = forecasts
        };

        _cache.Set(cacheKey, forecastResponse, TimeSpan.FromMinutes(60));
        _logger.LogInformation("Stored forecast in cache.");
        return forecastResponse;
    }
}
