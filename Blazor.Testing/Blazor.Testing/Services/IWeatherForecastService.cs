using Blazor.Testing.Models;

namespace Blazor.Testing.Services;

public interface IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecast>> GetForecastAsync();    
}