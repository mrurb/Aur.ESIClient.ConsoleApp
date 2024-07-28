using Aur.ESIClient.ConsoleApp.Core;

using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models.SSO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System;
using System.Net;
using System.Web;

namespace Aur.ESIClient.ConsoleApp;

public class Program
{
    static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        Startup(builder.Services, builder.Configuration);

        using IHost host = builder.Build();

        using var scope = host.Services.CreateScope();
        IEVEUser eVEData = scope.ServiceProvider.GetRequiredService<IEVEUser>();

        bool done = false;
        while (!done)
        {
            Console.WriteLine("Type add or done");
            string? v = Console.ReadLine() ?? throw new InvalidOperationException();

            switch (v)
            {
                
                case "add":
                    await eVEData.AddCharAsync();
                    break;
                case "done":
                    done = true;
                    break;
                default:
                    break;
            }
        }

        await host.RunAsync();
    }

    static void Startup(IServiceCollection services, ConfigurationManager configuration)
    {
        var options = configuration.GetSection("ESIConfig");
        services.AddEsi(options);
        services.AddSingleton<IEVEDataRepo, EVEDataRepo>();
        services.AddScoped<IEVELocation, EVELocation>();
        services.AddScoped<IEVEUser, EVEUser>();
        services.AddHostedService<Worker>();
    }
}
