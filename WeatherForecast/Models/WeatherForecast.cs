namespace WeatherForecast.Models
{
    public class Forecast
    {
        public string provider { get; set; }
        public double temperature_max { get; set; }
        public double temperature_min { get; set; }
    }

    public class ForecastResponse
    {
        public DateTime Date { get; set; }
        public List<Forecast> Forecast { get; set; }
    }
}
