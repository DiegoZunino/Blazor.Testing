namespace Blazor.Testing.IntegrationTests.SnapshotTesting;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
internal class CounterSnapshotTest : BlazorPageTest<Program>
{
    [Test]
    public async Task OnPageInitialization_VerifyPageContent()
    {
        // Arrange
        await Page.GotoPreRenderedAsync("counter");

        // Verify
        await Verify(await Page.ContentAsync(), "html");
        //    .PageScreenshotOptions(new PageScreenshotOptions { Quality = 50, Type = ScreenshotType.Jpeg });
    }
}