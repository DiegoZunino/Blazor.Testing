using Blazor.Testing.IntegrationTests.Services;

namespace Blazor.Testing.IntegrationTests.Pages;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
internal class WeatherStubServiceTest : BlazorPageTest<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddScoped<IWeatherForecastService>(_ => new StubWeatherForecastService(3));
        });
    }

    [Test]
    public async Task OnPageInitialization_Should_LoadAndDisplayData()
    {
        // Arrange
        await Page.GotoAsync("weather");

        // Act
        await Page.WaitForSelectorAsync("table>tbody>tr");

        // Assert
        var rows = await Page.Locator("table>tbody>tr").CountAsync();
        Assert.That(rows, Is.EqualTo(3));
    }
}
