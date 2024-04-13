﻿using System.Diagnostics;
using Microsoft.Playwright;

namespace Blazor.Testing.Tests.Utils;

public static class BlazorPageExtensions
{
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static Task<IResponse> GotoPreRenderedAsync(this IPage page, string url)
        => page.GotoAsync(url, new() { WaitUntil = WaitUntilState.NetworkIdle });
}
