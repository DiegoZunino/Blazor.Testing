namespace Blazor.Testing.IntegrationTests.Utils;

public class BlazorApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly Action<IWebHostBuilder> _configureWebHost;
    private IHost _host;

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }
    
    public BlazorApplicationFactory()
    {
    }

    public BlazorApplicationFactory(Action<IWebHostBuilder> configureWebHost)
    {
        _configureWebHost = configureWebHost;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        _configureWebHost?.Invoke(builder);        
        builder.UseUrls("https://127.0.0.1:0"); // Kestrel will select any free port.
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var testHost = builder.Build(); // Create the host for TestServer
        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel()); // Use Kestrel instead of TestServer

        // Create and start the Kestrel server before the test server,  
        // otherwise due to the way the deferred host builder works    
        // for minimal hosting, the server will not get "initialized    
        // enough" for the address it is listening on to be available.    
        // See https://github.com/dotnet/aspnetcore/issues/33846.    
        _host = builder.Build();
        _host.Start();
        
        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();
        ClientOptions.BaseAddress = addresses!.Addresses.Select(x => new Uri(x)).Last();

        // Return the host that uses TestServer, rather than the real one.  
        // Otherwise, the internals will complain about the host's server    
        // not being an instance of the concrete type TestServer.    
        // See https://github.com/dotnet/aspnetcore/pull/34702.   
        testHost.Start();
        return testHost;
    }

    private void EnsureServer()
    {
        if (_host is null)
        {
            using var _ = CreateDefaultClient();
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        _host?.Dispose();
    }
}