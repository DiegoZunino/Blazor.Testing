﻿using Blazor.Testing.Models;
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
        Services.AddSingleton(_weatherForecastService);
        ComponentFactories.AddStub<ForecastTable>();
    }

    [Test]
    public void ComponentRendersCorrectly()
    {
        // Act
        var cut = RenderComponent<Weather>();

        // Assert
        var heading = cut.Find("h1");
        Assert.That(heading.TextContent, Is.EqualTo("Weather"));
    }

    [Test]
    public void ComponentRetrievesWeatherForecastData()
    {
        // Act
        RenderComponent<Weather>();

        // Assert
        _weatherForecastService.Received(1).GetForecastAsync();
    }

    [Test]
    public void ComponentPassesRetrievedForecastsToTable()
    {
        // Arrange
        var forecasts = new List<WeatherForecast> { new () };
        _weatherForecastService.GetForecastAsync().Returns(forecasts);

        // Act
        var cut = RenderComponent<Weather>();

        // Assert
        var forecastTableComponent = cut.FindComponent<Stub<ForecastTable>>();
        Assert.That(forecastTableComponent.Instance.Parameters.Get(x => x.Forecasts),  Is.EqualTo(forecasts));
    }
}