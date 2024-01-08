using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using TransactionService.Scheduler.Repository.InternalApi.Implements;
using TransactionService.Scheduler.Repository.InternalApi.Interfaces;
using TransactionService.Scheduler.Schedulers;
using TransactionService.Scheduler.Services.Services.Implements;
using TransactionService.Scheduler.Services.Services.Interfaces;

var host = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((_, config) =>
    {
        // Delete all default configuration providers
        config.Sources.Clear();
        config
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
            .AddJsonFile("secrets.json", true, false)
            .AddEnvironmentVariables();
    })
    .ConfigureLogging(logging =>
    {
        // logging.SetMinimumLevel(LogLevel.Information);
        logging.AddNLog();
    })
    .ConfigureServices((_, services) =>
    {
        //Add Scheduler
        services.AddHostedService<TransactionScheduler>();
        
        //Add Dependency
        services.AddScoped<IMathService, MathService>();
        services.AddScoped<IPreTransactionService, PreTransactionService>();
        services.AddScoped<IDepositServiceApi, DepositServiceApi>();
    })
    .Build();

//get environment
// var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

await host.RunAsync();

