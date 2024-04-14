namespace Blazor.Testing.Tests.End2End;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
internal class ComplexCounterPageTest : BlazorPageTest<Program>
{
    [Test]
    public async Task WhenButtonIsClickedMoreThenThreeTimes_Should_ContinueIncrementCurrentCount()
    {
        // Arrange
        await Page.GotoPreRenderedAsync("complex-counter");

        //Act
        var button = Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { Name = "Click me" });
        await button.ClickAsync();
        await button.ClickAsync();
        await button.ClickAsync();
        await button.ClickAsync();
        await button.ClickAsync();
        
        // Assert
        ILocator status = Page.GetByRole(AriaRole.Status);
        await Expect(status).ToHaveTextAsync("Current count: 5");
    }
}
