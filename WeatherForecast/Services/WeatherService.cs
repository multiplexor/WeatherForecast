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

    public async Task<ForecastResponse?> GetForecast(DateTime date, string city, string country)
    {
        if (_weatherClients == null || !_weatherClients.Any())
        {
            return null;
        }

        var cacheKey = $"{date}-{city}-{country}";
        if (_cache.TryGetValue(cacheKey, out ForecastResponse? cachedForecast))
        {
            return cachedForecast;
        }

        (double latitude, double longitude)? location;
        try
        {
            location = await _locationService.GetLocation(city, country);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving location: {ErrorMessage}", ex.Message);
            return null;
        }
        if (location == null)
            return null;

        List<Task<Forecast?>> forecastTasks = new List<Task<Forecast?>>();
        foreach (var weather in _weatherClients)
        {
            forecastTasks.Add(Task.Run(async () =>
            {
                try
                {
                    return await weather.GetWeatherForecast(date, location.Value.latitude, location.Value.longitude);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while retrieving weather forecast: {ErrorMessage}", ex.Message);
                    return null;
                }
            }));
        }

        await Task.WhenAll(forecastTasks);

        List<Forecast> forecasts = forecastTasks.Where(task => task.Result != null).Select(task => task.Result).ToList();

        var forecastResponse = new ForecastResponse
        {
            Date = date,
            Forecast = forecasts
        };

        _cache.Set(cacheKey, forecastResponse, TimeSpan.FromMinutes(60));

        return forecastResponse;
    }
}
