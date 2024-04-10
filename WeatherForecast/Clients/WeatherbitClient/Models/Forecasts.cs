namespace WeatherForecast.Clients.WeatherbitClient.Models
{
    public class Forecasts
    {
        public List<Forecast> data { get; set; }
    }

    public class Forecast
    {
        public long ts { get; set; }
        public double min_temp { get; set; }
        public double max_temp { get; set; }
    }
}
