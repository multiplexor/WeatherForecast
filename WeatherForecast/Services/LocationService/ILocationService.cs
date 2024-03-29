namespace WeatherForecast.Services.LocationService
{
    public interface ILocationService
    {
        Task<(double latitude, double longitude)?> GetLocation(string city, string country);
    }
}