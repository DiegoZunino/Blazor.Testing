namespace Blazor.Testing.Tests.Components.Pages;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class ComplexCounterTest : TestContext
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
    public void WhenButtonIsClicked_Should_IncrementCurrentCount()
    {
        // Act
        var cut = RenderComponent<ComplexCounter>();
        var increaseCounterButton = cut.Find("button");
        increaseCounterButton.Click();
        
        // Assert
        var currentCounterContent = cut.Find("p[role='status']");
        Assert.That(currentCounterContent.TextContent, Is.EqualTo("Current count: 1"));
    }
    
    [Test]
    public async Task WhenButtonIsClickedThreeTimes_Should_InvokeAlertService()
    {
        // Act
        var cut = RenderComponent<ComplexCounter>();
        var increaseCounterButton = cut.Find("button");
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        
        // Assert
        await _jsObjectReferenceMock.Received(1).InvokeVoidAsync("showAlert", Arg.Is<object[]>(x => x.Contains("Counter is now 3!")));
    }
    
    [Test]
    public void WhenButtonIsClickedMoreThenThreeTimes_Should_ContinueaIncrementCurrentCount()
    {
        // Act
        var cut = RenderComponent<ComplexCounter>();
        var increaseCounterButton = cut.Find("button");
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        
        // Assert
        var currentCounterContent = cut.Find("p[role='status']");
        Assert.That(currentCounterContent.TextContent, Is.EqualTo("Current count: 5"));
    }
}