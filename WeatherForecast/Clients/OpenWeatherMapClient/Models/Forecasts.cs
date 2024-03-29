namespace WeatherForecast.Clients.OpenWeatherMapClient.Models
{
    public class Forecasts
    {
        public List<Forecast> daily { get; set; }
    }

    public class Forecast
    {
        public long dt { get; set; }
        public Temperatures temp { get; set; }
    }

    public class Temperatures
    {
        public double min { get; set; }
        public double max { get; set; }
    }
}
