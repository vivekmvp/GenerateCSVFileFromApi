# Generate a CSV File From Api
Generate a CSV File from .Net Core Api.  Export to CSV File from .Net Core Web Api.  Download CSV File from .Net Core Web Api

----

A Demo showing how to export data to CSV file and download from Swagger and Postman.  I will be using out-of-box .Net core api template to demo this.


<kbd>![image](https://user-images.githubusercontent.com/30829678/195450750-b6c91adc-e4fe-4a81-9bd9-694fd6b2316f.png)</kbd>


<kbd>![image](https://user-images.githubusercontent.com/30829678/195451181-61caf241-88dc-47b1-88d8-04b134b2fa45.png)</kbd>


<kbd>![image](https://user-images.githubusercontent.com/30829678/195451515-170229db-0892-42f5-89a4-74e32f0b545c.png)</kbd>

<kbd>![image](https://user-images.githubusercontent.com/30829678/195453010-ae2404e0-f063-44b5-ad03-50c042ef850e.png)</kbd>


```C#
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
```
