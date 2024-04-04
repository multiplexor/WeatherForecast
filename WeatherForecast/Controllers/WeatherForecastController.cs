using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Interfaces;

namespace WeatherForecast.Controllers
{
    [ApiController]
    [Route("/v1/forecast")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime dateTime, [FromQuery] string city, [FromQuery] string country)
        {
            if (dateTime == default(DateTime) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                _logger.LogError("Invalid request parameters.");
                return BadRequest("All parameters are required.");
            }

            DateTime date = dateTime.ToUniversalTime().Date;
            if (date > DateTime.UtcNow.AddDays(8).Date || date < DateTime.UtcNow.Date)
            {
                _logger.LogError("Invalid date parameter.");
                return BadRequest("Invalid date. Date should be today or no more than 8 days ahead.");
            }

            var forecast = await _weatherService.GetForecast(date, city, country);
            if (forecast == null)
            {
                _logger.LogError("Failed to retrieve forecast.");
                return StatusCode(500, "Failed to retrieve forecast");
            }

            _logger.LogInformation("Forecast retrieved successfully.");
            return Ok(forecast);
        }
    }
}
