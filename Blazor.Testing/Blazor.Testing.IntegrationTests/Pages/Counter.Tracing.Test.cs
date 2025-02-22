namespace Blazor.Testing.IntegrationTests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
internal class CounterTracingTest : BlazorPageTest<Program>
{
    [Test]
    public async Task WhenButtonIsClickedMoreThenThreeTimes_Should_ContinueIncrementCurrentCount()
    {
        // Arrange
        await Context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        // Act
        await Page.GotoPreRenderedAsync("counter");
        var button = Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { Name = "Click me" });
        await button.ClickAsync();
        await button.ClickAsync();
        await button.ClickAsync();
        await button.ClickAsync();
        await button.ClickAsync();

        // Assert
        ILocator status = Page.GetByRole(AriaRole.Status);
        await Expect(status).ToHaveTextAsync("Current count: 5");

        await Context.Tracing.StopAsync(new TracingStopOptions
        {
            Path = nameof(WhenButtonIsClickedMoreThenThreeTimes_Should_ContinueIncrementCurrentCount) + ".trace.zip"
        });

        // View trace:
        // pwsh ./Blazor.Testing.IntegrationTests/bin/Debug/net8.0/playwright.ps1 show-trace ./Blazor.Testing.IntegrationTests/bin/Debug/net8.0/WhenButtonIsClickedMoreThenThreeTimes_Should_ContinueIncrementCurrentCount.trace.zip
    }
}
