namespace Blazor.Testing.Tests.Components.Core;

public class HelloWorldTests : TestContext
{
    [Test]
    public void RendersCorrectly()
    {
        // Arrange
        var nameToGreet = "Fabio";
        
        // Act
        var cut = RenderComponent<HelloWorld>(p 
            => p.Add(x => x.Name, nameToGreet));
        
        // Assert
        cut.MarkupMatches($"<h1>Hello, {nameToGreet}!</h1>");
    }
}