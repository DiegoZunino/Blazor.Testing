namespace Blazor.Testing.IntegrationTests.SnapshotTesting;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
internal class CounterSnapshotTest : BlazorPageTest<Program>
{
    [Test]
    public async Task OnPageInitialization_Verify()
    {
        // Arrange
        await Page.GotoPreRenderedAsync("counter");

        // Verify page content only semantically
        // string html = await Page.ContentAsync();
        //await Verify(html, "html");

        // Verify page and create screenshot
        await Verify(Page).PageScreenshotOptions(new()
        {
            Quality = 50,
            Type = ScreenshotType.Jpeg,
        });
    }
}