using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blazor.Testing.IntegrationTests.SnapshotTesting;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
internal class CounterSnapshotTest : BlazorPageTest<Program>
{
    protected override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ViewportSize = new ViewportSize()
            {
                Width = 600,
                Height = 400
            }
        };
    }

    [Test]
    public async Task OnPageInitialization_VerifyPageContent()
    {
        // Arrange
        await Page.GotoPreRenderedAsync("counter");
        
        // Act
        // var button = Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { Name = "Click me" });
        // await button.ClickAsync();
        
        // Assert
        await Verify(Page).PageScreenshotOptions(new PageScreenshotOptions
        {
            Quality = 50,
            Type = ScreenshotType.Jpeg
        });
    }
}