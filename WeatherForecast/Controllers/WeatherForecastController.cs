using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Services.WeatherService;

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
                return BadRequest("All parameters are required.");
            }

            DateTime date = dateTime.ToUniversalTime().Date;
            if (date > DateTime.UtcNow.AddDays(8).Date || date < DateTime.UtcNow.Date)
            {
                return BadRequest("Invalid date. Date should be today or no more than 8 days ahead.");
            }

            var forecast = await _weatherService.GetForecast(date, city, country);
            if (forecast == null)
            {
                return StatusCode(500, "Failed to retrieve forecast");
            }

            return Ok(forecast);
        }
    }
}
