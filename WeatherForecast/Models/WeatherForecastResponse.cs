using System;
using WeatherForecast.Services.LocationService;

namespace WeatherForecast.Models
{
    public class WeatherForecastResponse
    {
        public DateTime Date { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<ForecastResponse> Forecast { get; set; }
    }

    public class ForecastResponse
    {
        public static ForecastResponse FailureResult(string provider, string errorMessage)
        {
            return new ForecastResponse { Success = false, Provider = provider, ErrorMessage = errorMessage };
        }

        public static ForecastResponse SuccessResult(string provider, double temperature_min, double temperature_max)
        {
            return new ForecastResponse { Success = true, Provider = provider, Temperature_min = temperature_min, Temperature_max = temperature_max };
        }

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public string Provider { get; set; }
        public double Temperature_max { get; set; }
        public double Temperature_min { get; set; }
    }
}
