using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TransactionService.Scheduler.Helper.Expressions;
using TransactionService.Scheduler.Services.Services.Implements;

namespace TransactionService.Scheduler.Schedulers;

public class TransactionScheduler : IHostedService, IDisposable
{
    private readonly ILogger<TransactionScheduler> _logger;
    private readonly IMathService _mathService;
    private readonly TimeSpan _scheduleFrequency;
    private Timer? _timer;
    private int _countState;

    public TransactionScheduler(ILogger<TransactionScheduler> logger, IMathService mathService,
        IConfiguration configuration)
    {
        _logger = logger;
        _mathService = mathService;
        var getSetting = configuration.GetSection("ScheduleFrequency");
        var getSetting2 = configuration.GetValue<string>("ScheduleFrequency", "5s")!;
        _scheduleFrequency = TimeSpanExpressions.ParseToTimeSpan(getSetting2);
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Transaction scheduler is starting at {DateTime.Now:h:mm:ss tt zz}");
        _countState = 0;
        _timer = new Timer(AsyncWrapper, null, TimeSpan.Zero, _scheduleFrequency);

        return Task.CompletedTask;
    }

    private void AsyncWrapper(object? state) => Task.Run(Execution);

    private async Task Execution()
    {
        _logger.LogInformation($"Transaction scheduler executing at {DateTime.Now:h:mm:ss tt zz}");
        _countState = await _mathService.Count(_countState, 2);
        _logger.LogInformation($"count at {_countState}");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Transaction scheduler is stopping at {DateTime.Now:h:mm:ss tt zz}");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}