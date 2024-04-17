using Blazor.Testing.Models;
using Blazor.Testing.Services;
using Bunit.TestDoubles;

namespace Blazor.Testing.Tests.Components.Pages;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class WeatherTests : TestContext
{
    private readonly IWeatherForecastService _weatherForecastService = Substitute.For<IWeatherForecastService>();

    [SetUp]
    public void Setup()
    {
        #region Component Factories
        
        ComponentFactories.AddStub<ForecastTable>();
        
        #endregion
        
    }

    [Test]
    public void ComponentRendersCorrectly()
    {
        // Arrange
        Services.AddSingleton(_weatherForecastService);
        
        // Act
        var cut = RenderComponent<Weather>();

        // Assert
        var heading = cut.Find("h1");
        Assert.That(heading.TextContent, Is.EqualTo("Weather"));
    }

    [Test]
    public void ComponentRetrievesWeatherForecastData()
    {
        // Arrange
        Services.AddSingleton(_weatherForecastService);
        
        // Act
        RenderComponent<Weather>();

        // Assert
        _weatherForecastService.Received(1).GetForecastAsync();
    }

    [Test]
    public void ComponentPassesRetrievedForecastsToTable()
    {
        // Arrange
        List<WeatherForecast> forecasts = [new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now), TemperatureC = 19, Summary = "Chill"}];
        _weatherForecastService.GetForecastAsync().Returns(forecasts);
        Services.AddSingleton(_weatherForecastService);

        // Act
        var cut = RenderComponent<Weather>();

        // Assert
        var forecastTableComponent = cut.FindComponent<Stub<ForecastTable>>();
        Assert.That(forecastTableComponent.Instance.Parameters.Get(x => x.Forecasts),  Is.EqualTo(forecasts));
    }
}