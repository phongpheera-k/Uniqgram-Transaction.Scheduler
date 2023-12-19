using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using TransactionService.Scheduler.Repository.InternalApi.Implements;
using TransactionService.Scheduler.Repository.InternalApi.Interfaces;
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
        services.AddScoped<IPreTransactionService, PreTransactionService>();
        services.AddScoped<IDepositServiceApi, DepositServiceApi>();
    })
    .Build();

await host.RunAsync();

