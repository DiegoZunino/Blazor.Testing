namespace Blazor.Testing.IntegrationTests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
internal class CounterManualArrangeTests
{
    [Test]
    public async Task WhenButtonIsClickedMoreThenThreeTimes_Should_IncrementCurrentCount()
    {
        // Arrange
        await using var host = new BlazorApplicationFactory<Program>();
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        
        var contextOptions = new BrowserNewContextOptions
        {
            BaseURL = host.ServerAddress,
            IgnoreHTTPSErrors = true,
        };

        var context = await browser.NewContextAsync(contextOptions);
        var page = await context.NewPageAsync();

        // Go to the counter page, and waits till the network is idle.
        // This is needed when pre-rendering is enabled and using Blazor Server,
        // since the page is not interactive until the SignalR connection to the
        // backend has been established.
        await page.GotoAsync("counter", new() { WaitUntil = WaitUntilState.NetworkIdle });

        // Act
        var button = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { Name = "Click me" });
        await button.ClickAsync();
        await button.ClickAsync();
        await button.ClickAsync();
        await button.ClickAsync();
        await button.ClickAsync();

        // Assert
        var status = page.GetByRole(AriaRole.Status);
        await Expect(status).ToHaveTextAsync("Current count: 5");
    }
}
