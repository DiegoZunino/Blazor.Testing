namespace Blazor.Testing.IntegrationTests.End2End;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
internal class ComplexCounterPageManualTest
{
    [Test]
    public async Task WhenButtonIsClickedMoreThenThreeTimes_Should_IncrementCurrentCount()
    {
        // Arrange
        // Runs Blazor App referenced by Program, making it
        // available on 127.0.0.1 on a random free port.
        await using BlazorApplicationFactory<Program> host = new();

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        BrowserNewContextOptions contextOptions = new BrowserNewContextOptions
        {
            // Assigns the base address of the host
            // (cannot be hardcoded due to random chosen port)
            BaseURL = host.ServerAddress,
            // BAF/WAF uses dotnet dev-cert for HTTPS. If
            // that is not trusted on your CI pipeline, this ensures
            // that tests will continue working.
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
