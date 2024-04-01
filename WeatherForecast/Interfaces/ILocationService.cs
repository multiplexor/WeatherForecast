namespace WeatherForecast.Interfaces
{
    public interface ILocationService
    {
        Task<(double latitude, double longitude)?> GetLocation(string city, string country);
    }
}