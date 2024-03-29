namespace WeatherForecast.Clients.OpenMeteoClient.Models
{
    public class ForecastResponse
    {
        public Forecasts? daily { get; set; }
    }

    public class Forecasts
    {
        public List<DateTime>? time { get; set; }
        public List<double> temperature_2m_max { get; set; }
        public List<double> temperature_2m_min { get; set; }
    }
}
