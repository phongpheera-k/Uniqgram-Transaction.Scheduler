using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using TransactionService.Scheduler.Schedulers;
using TransactionService.Scheduler.Services.Services.Implements;
using TransactionService.Scheduler.Services.Services.Interfaces;

var host = Host.CreateDefaultBuilder(args)
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
    })
    .Build();

await host.RunAsync();