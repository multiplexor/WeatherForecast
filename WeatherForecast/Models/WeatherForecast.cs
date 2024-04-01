namespace WeatherForecast.Models
{
    public class Forecast
    {
        public string Provider { get; set; }
        public double Temperature_max { get; set; }
        public double Temperature_min { get; set; }
    }

    public class ForecastResponse
    {
        public DateTime Date { get; set; }
        public List<Forecast> Forecast { get; set; }
    }
}
