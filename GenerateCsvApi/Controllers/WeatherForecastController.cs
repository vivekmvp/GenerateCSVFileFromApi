using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Text;

namespace GenerateCsvApi.Controllers
{
    [Produces("application/json")]
    [Route("weather")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("forecast")]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost]
        [Route("export-forecast")]
        [Produces("text/csv")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GenerateCsv()
        {
            //Get the data from DB Goes here
            //To avoid complexity, I am just representing out of box data
            List<WeatherForecast> lstWeatherForecasts =
                Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToList();

            //Transform data to CSV String
            string data2csvString = await WeatherForecastListToCSV(lstWeatherForecasts);

            //Transform CSV String to Byte Array
            var fileBytes = Encoding.UTF8.GetBytes(data2csvString);

            //Retrun the CSV File
            //Note - "G" + DateTime.Now.Ticks + ".csv" is used to generate unique file name
            //You can use any other way to generate unique file name
            return File(fileBytes, "text/csv", "G" + DateTime.Now.Ticks + ".csv");
        }


        //I choose manual way to Transform data to CSV so that we can have full control
        //Keep it simple rule and full control on how to generate csv file.
        private async Task<string> WeatherForecastListToCSV(List<WeatherForecast> weatherForecasts)
        {
            StringBuilder cvsBuilder = new StringBuilder();

            //Add Header Columns
            cvsBuilder.AppendLine("Date,Centigrade,Fahrenheit,Real Feel");

            //Iterate through data
            foreach (WeatherForecast forecast in weatherForecasts)
            {
                cvsBuilder.Append(forecast.Date.ToShortDateString());
                cvsBuilder.Append(",");
                cvsBuilder.Append(forecast.TemperatureC);
                cvsBuilder.Append(",");
                cvsBuilder.Append(forecast.TemperatureF);
                cvsBuilder.Append(",");
                cvsBuilder.Append(forecast.Summary);                
                cvsBuilder.Append("\n"); //New line character
            }

            return await Task.FromResult(cvsBuilder.ToString());
        }

    }
}