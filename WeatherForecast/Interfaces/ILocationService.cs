using WeatherForecast.Services.LocationService.Models;

namespace WeatherForecast.Interfaces
{
    public interface ILocationService
    {
        Task<LocationResponse> GetLocation(string city, string country);
    }
}