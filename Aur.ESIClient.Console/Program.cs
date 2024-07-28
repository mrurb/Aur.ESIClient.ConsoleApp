using Aur.ESIClient.ConsoleApp.Core;

using ESI.NET;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System;
using System.Net;

namespace Aur.ESIClient.ConsoleApp;

public class Program
{
    static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        IHostEnvironment env = builder.Environment;

        Startup(builder.Services, builder.Configuration);

        using IHost host = builder.Build();

        using var scope = host.Services.CreateScope();
        IEsiClient esiClient = scope.ServiceProvider.GetService<IEsiClient>() ?? throw new InvalidOperationException();

        Guid state = Guid.NewGuid();

        string url = esiClient.SSO.CreateAuthenticationUrl(state: state.ToString());
        Console.WriteLine(url);

        HttpListener listener = new HttpListener() { };

        listener.Prefixes.Add("http://localhost:8080/callback/");

        listener.Start();
        Console.WriteLine("Listening...");
        HttpListenerContext context = listener.GetContext();
        HttpListenerRequest request = context.Request;
        // Obtain a response object.
        HttpListenerResponse response = context.Response;
 

        
        


        
        Console.ReadLine();

        await host.RunAsync();
    }

    static void Startup(IServiceCollection services, ConfigurationManager configuration)
    {
        var options = configuration.GetSection("ESIConfig");
        services.AddEsi(options);

        services.AddHostedService<Worker>();
    }
}
