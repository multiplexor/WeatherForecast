using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Interfaces;
using WeatherForecast.Models;

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
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                _logger.LogError("Invalid request parameters. dateTime should be in UTC format.");
                return BadRequest("dateTime should be in UTC format.");
            }

            DateTime date = dateTime.Date;
            if (date > DateTime.UtcNow.AddDays(16).Date || date < DateTime.UtcNow.Date)
            {
                _logger.LogError("Invalid date parameter.");
                return BadRequest("Invalid date. Date should be today or no more than 16 days ahead.");
            }

            WeatherForecastResponse forecast;
            try
            {
                forecast = await _weatherService.GetForecast(date, city, country);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving forecast.");
                return StatusCode(500, "An error occurred while retrieving forecast.");
            }

            _logger.LogInformation("Forecast retrieved successfully.");
            return Ok(forecast);
        }
    }
}
