namespace Blazor.Testing.IntegrationTests.Services;

internal class StubWeatherForecastService(int forecastsToReturn) : IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecast>> GetForecastAsync()
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        var forecasts = Enumerable
            .Range(1, forecastsToReturn)
            .Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            });

        return Task.FromResult(forecasts);
    }
}
