using Blazor.Testing.Models;

namespace Blazor.Testing.Services;

public class WeatherForecastService : IWeatherForecastService
{
    public async Task<IEnumerable<WeatherForecast>> GetForecastAsync()
    {
        await Task.Delay(500);

        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //return Enumerable.Range(1, new Random().Next(1, 10)).Select(index => new WeatherForecast    
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray();
    }
}