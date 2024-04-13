namespace Blazor.Testing.Tests.Components.Pages;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class CounterTest : TestContext
{
    private readonly IJSRuntime _jsRuntimeMock = Substitute.For<IJSRuntime>();
    private readonly IJSObjectReference _jsObjectReferenceMock = Substitute.For<IJSObjectReference>();
    
    [SetUp]
    public void Setup()
    {
        _jsRuntimeMock.InvokeAsync<IJSObjectReference>("import", Arg.Any<object[]>())
            .Returns(_jsObjectReferenceMock);
        Services.AddSingleton(_jsRuntimeMock);
    }
    
    [Test]
    public async Task CounterIntegrationTest()
    {
        // Act
        var cut = RenderComponent<ComplexCounter>();
        var increaseCounterButton = cut.Find("button");
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        
        // Assert
        var currentCounterContent = cut.Find("p[role='status']");
        
        Assert.That(currentCounterContent.TextContent, Is.EqualTo("Current count: 3"));
        await _jsObjectReferenceMock.Received(1).InvokeVoidAsync("showAlert", Arg.Is<object[]>(x => x.Contains("Counter is now 3!")));
    }
}