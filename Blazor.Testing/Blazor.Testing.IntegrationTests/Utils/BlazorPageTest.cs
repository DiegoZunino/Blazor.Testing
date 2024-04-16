namespace Blazor.Testing.IntegrationTests.Utils;

public class BlazorPageTest<TProgram> : BrowserTest where TProgram : class
{
    private BlazorApplicationFactory<TProgram> _host;
    protected IBrowserContext Context { get; private set; } = null!;
    protected IPage Page { get; private set; } = null!;
    
    private BlazorApplicationFactory<TProgram> Host
    {
        get
        {
            _host ??= CreateHostFactory() ?? new BlazorApplicationFactory<TProgram>(ConfigureWebHost);
            return _host;
        }
    }

    protected virtual BlazorApplicationFactory<TProgram> CreateHostFactory() => new (ConfigureWebHost);
    protected virtual BrowserNewContextOptions ContextOptions() => null!;
    protected virtual void ConfigureWebHost(IWebHostBuilder builder) { }

    [SetUp]
    public async Task Setup()
    {
        VerifyPlaywright.Initialize();
        
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
            await Context.DisposeAsync().ConfigureAwait(false);
            await currentHost.DisposeAsync().ConfigureAwait(false);
        }
    }
}
