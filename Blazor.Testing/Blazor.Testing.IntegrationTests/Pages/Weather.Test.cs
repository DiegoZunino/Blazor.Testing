namespace Blazor.Testing.IntegrationTests.Pages;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
internal class WeatherTest : BlazorPageTest<Program>
{
    [Test]
    public async Task OnPageInitialization_Should_LoadAndDisplayData()
    {
        // Arrange
        await Page.GotoAsync("weather");

        // Act
        await Page.WaitForSelectorAsync("table>tbody>tr");

        // Assert
        var rows = await Page.Locator("table>tbody>tr").CountAsync();        
        Assert.That(rows, Is.EqualTo(5));
    }
}