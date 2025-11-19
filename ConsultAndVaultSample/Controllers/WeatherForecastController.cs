using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConsulAndVaultSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptionsMonitor<SampleSetting> _setting;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptionsMonitor<SampleSetting> setting)
        {
            _logger = logger;
            _setting = setting;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var x = _setting.CurrentValue.AnotherValue.ToString(); // Use the setting from Consul KV
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
