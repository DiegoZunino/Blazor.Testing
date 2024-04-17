namespace Blazor.Testing.Tests.Components.Pages;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class CounterTest : TestContext
{
    private readonly IJSRuntime _jsRuntimeMock = Substitute.For<IJSRuntime>();
    
    [SetUp]
    public void Setup()
    {
        Services.AddSingleton(_jsRuntimeMock);
    }
    
    [Test]
    public void WhenButtonIsClicked_Should_IncrementCurrentCount()
    {
        // Act
        var cut = RenderComponent<Counter>();
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
        var cut = RenderComponent<Counter>();
        var increaseCounterButton = cut.Find("button");
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        increaseCounterButton.Click();
        
        // Assert
        await _jsRuntimeMock.Received(1).InvokeVoidAsync("showAlert", Arg.Is<object[]>(x => x.Contains("Counter is now 3!")));
    }
    
    [Test]
    public void WhenButtonIsClickedMoreThenThreeTimes_Should_ContinueIncrementCurrentCount()
    {
        // Act
        var cut = RenderComponent<Counter>();
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