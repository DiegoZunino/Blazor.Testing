namespace Blazor.Testing.IntegrationTests.Utils;

public static class BlazorPageExtensions
{
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static Task<IResponse> GotoPreRenderedAsync(this IPage page, string url)
        => page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
}
