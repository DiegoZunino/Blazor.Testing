﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Blazor.Testing.Tests.Utils;

public class BlazorPageTest<TProgram> : BrowserTest where TProgram : class
{
    private BlazorApplicationFactory<TProgram> _host;

    public IBrowserContext Context { get; private set; } = null!;

    public IPage Page { get; private set; } = null!;

    public BlazorApplicationFactory<TProgram> Host
    {
        get
        {
            _host ??= CreateHostFactory() ?? new BlazorApplicationFactory<TProgram>(ConfigureWebHost);
            return _host;
        }
    }

    public virtual BlazorApplicationFactory<TProgram> CreateHostFactory() => new(ConfigureWebHost);

    public virtual BrowserNewContextOptions ContextOptions() => null!;

    protected virtual void ConfigureWebHost(IWebHostBuilder builder) { }

    [SetUp]
    public async Task PageSetup()
    {
        var options = ContextOptions() ?? new BrowserNewContextOptions();
        options.BaseURL = Host.ServerAddress;
        options.IgnoreHTTPSErrors = true;

        Context = await NewContext(options).ConfigureAwait(false);
        Page = await Context.NewPageAsync().ConfigureAwait(false);
    }

    [TearDown]
    public async Task HostTearDown()
    {
        if (_host is { } currentHost)
        {
            _host = null;

            // Navigate to about:blank to ensure any SignalR
            // connections are dropped.
            //await Page.GotoAsync("about:blank");
            await Context.DisposeAsync().ConfigureAwait(false);
            await currentHost.DisposeAsync().ConfigureAwait(false);
        }
    }
}
