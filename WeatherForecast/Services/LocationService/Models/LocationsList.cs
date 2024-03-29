namespace WeatherForecast.Services.LocationService.Models
{
    public class LocationsList
    {
        public List<CityInfo> results { get; set; }
    }

    public class CityInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string country { get; set; }
    }
}
